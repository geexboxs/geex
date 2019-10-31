
import { GraphQLModule } from "@graphql-modules/core";
import { ProviderScope, Type } from "@graphql-modules/di";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { createConnection } from "mongoose";
import { environment } from "../../environments/environment";
import { IGeexContext, IGeexServerConfig, IAuthConfig, IUserContext } from "../../types";
import { GeexServerConfigToken, UserModelToken, AuthConfigToken as UserModuleConfigToken } from "../../shared/tokens";
import { Model, Enforcer, newEnforcer } from "casbin";
import MongooseAdapter = require("@elastic.io/casbin-mongoose-adapter");
import { CasbinModel } from "./utils/casbin-model";
import { UserResolver } from "./user.resolver";
import { buildSchemaSync, ClassType } from "type-graphql";
import { getModelForClass, ReturnModelType } from "@typegoose/typegoose";
import { User } from "./models/user.model";
import { PasswordHasher } from "./utils/password-hasher";
import { GraphQLLocalStrategy } from "graphql-passport";
import { Passport } from "passport";
import passport = require("passport");
import ioredis = require("ioredis");
import { I18N } from "../../shared/utils/i18n";
import { Jwt } from "../../shared/utils/jwt";
import { SessionStore } from "./models/session.model";
import json5 = require("json5");
import { Strategy as JwtStrategy, ExtractJwt } from "passport-jwt";
import { RbacAuthChecker } from "../../shared/utils/rbac-auth-checker";
import { EmailSender } from "../../shared/utils/email-sender";

const resolvers: [ClassType] = [UserResolver];
let enforcer: Enforcer;
async function preInitialize() {
    const adapter = await MongooseAdapter.newAdapter(environment.connections.mongo);
    enforcer = await newEnforcer(CasbinModel, adapter);
    enforcer.enableAutoSave(true);
    await enforcer.loadPolicy();
    return;
}
async function postInitialize(self: GraphQLModule) {
    // tslint:disable-next-line: only-arrow-functions
    passport.serializeUser(function (user, done) {
        done(undefined, json5.stringify(user));
    });

    // tslint:disable-next-line: only-arrow-functions
    passport.deserializeUser(function (user: string, done) {
        done(undefined, json5.parse(user));
    });
    passport.use("local", new GraphQLLocalStrategy(async (username, password, done) => {
        const userModel = self.injector.get<ReturnModelType<typeof User>>(UserModelToken);
        const passwordHasher = self.injector.get(PasswordHasher);
        const userDoc = await userModel.findOne({
            username,
            passwordHash: passwordHasher.hash(password),
        }).exec();
        if (userDoc === null) {
            throw new Error(I18N.message.userNotFound);
        } else {
            const user = userDoc.toObject({ getters: true }) as IUserContext;
            done(undefined, user);
        }
    }));
    passport.use("jwt", new JwtStrategy({
        jwtFromRequest: ExtractJwt.fromAuthHeaderAsBearerToken(),
        secretOrKey: environment.authConfig.tokenSecret,
        issuer: environment.hostname,
        audience: "accessToken",
    }, async (jwt_payload, done) => {
        const userId = jwt_payload && jwt_payload.sub;
        if (userId === undefined) {
            done(undefined, undefined);
        }
        const sessionStore = self.injector.get(SessionStore);
        const session = await sessionStore.get(userId);
        if (session === null || session.expired()) {
            await sessionStore.del(userId);
            throw new Error(I18N.message.sessionExpired);
        } else {
            done(undefined, session && session.user);
        }
    }));
    return;
}
export const UserModule = (async () => {
    await preInitialize();
    const self = new GraphQLModule<IAuthConfig, ExpressContext, IGeexContext>({
        providers: () => [
            SessionStore,
            {
                provide: ioredis,
                useValue: new ioredis(environment.connections.redis),
            },
            {
                provide: Jwt,
                useValue: new Jwt(environment.authConfig.tokenSecret, `${environment.hostname}`),
            },
            {
                provide: Passport,
                useValue: passport.initialize(),
            },
            {
                provide: UserModelToken,
                useFactory: (injector) => {
                    const userModel = getModelForClass(User);
                    userModel.setDependentImplementionForModel("roles", function (this: User) {
                        return injector.get(Enforcer).getRolesForUser(this.id)
                    });
                    userModel.setDependentImplementionForModel<"permissions">("permissions", function (this: User) {
                        return injector.get(Enforcer).getPermissionsForUser(this.id);
                    });
                    return userModel;
                },
            },
            {
                provide: UserModuleConfigToken,
                useValue: environment.authConfig,
            },
            {
                provide: PasswordHasher,
                useFactory: (injector) => new PasswordHasher(environment.authConfig.tokenSecret),
            },
            {
                provide: EmailSender,
                useValue: new EmailSender(environment.connections.smtp.sendAs, {
                    secure: environment.connections.smtp.secure,
                    auth: {
                        user: environment.connections.smtp.username,
                        pass: environment.connections.smtp.password,
                    },
                    host: environment.connections.smtp.host,
                    port: environment.connections.smtp.port,
                }),
            },
            {
                provide: Enforcer,
                useValue: enforcer,
            },
            ...resolvers,
        ],
        extraSchemas: [
            buildSchemaSync({
                container: ({ ...args }) => {
                    return {
                        get(someClass, resolverData) {
                            return (resolverData.context as IGeexContext).injector.get(someClass);
                        },
                    };
                },
                resolvers,
                authChecker: RbacAuthChecker,
            }),
        ],
    }, environment.authConfig);

    await postInitialize(self);
    return self;
})();
