
import { GraphQLModule } from "@graphql-modules/core";
import { ProviderScope, Type } from "@graphql-modules/di";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { createConnection } from "mongoose";
import { environment } from "../../environments/environment";
import { IGeexContext, IGeexServerConfig, IAuthConfig } from "../../types";
import { GeexServerConfigToken, UserModelToken, AuthConfigToken as UserModuleConfigToken } from "../../shared/tokens";
import { Model, Enforcer, newEnforcer } from "casbin";
import * as MongooseAdapter from "@elastic.io/casbin-mongoose-adapter";
import { CasbinModel } from "./utils/casbin-model";
import { UserResolver } from "./user.resolver";
import { buildSchemaSync, ClassType } from "type-graphql";
import { getModelForClass, ReturnModelType } from "@typegoose/typegoose";
import { User } from "./models/user.model";
import { PasswordHasher } from "./utils/password-hasher";
import { GraphQLLocalStrategy } from "graphql-passport";
import passport, { Passport } from "passport";
import * as ioredis from "ioredis";
import { I18N } from "../../shared/utils/i18n";
import { Jwt } from "../../shared/utils/jwt";
import { SessionStore } from "./models/session.model";
import * as json5 from "json5";
import { Strategy as JwtStrategy, ExtractJwt } from "passport-jwt";
import { RbacAuthChecker } from "../../shared/utils/rbac-auth-checker";

const resolvers: [ClassType] = [UserResolver];
let enforcer: Enforcer;
async function preInitialize() {
    const adapter = await MongooseAdapter.default.newAdapter(environment.connections.mongo);
    enforcer = await newEnforcer(CasbinModel, adapter);
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
        const matchingUser = await userModel.findOne({
            username,
            passwordHash: passwordHasher.hash(password),
        }).exec();

        const error = matchingUser ? undefined : new Error(I18N.message.userNotFound);
        const enforcer = self.injector.get(Enforcer);
        matchingUser!.roles = await enforcer.getRolesForUser(matchingUser!.id);
        done(error, matchingUser && matchingUser.toObject({ getters: true }));
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
        const error = session ? undefined : new Error(I18N.message.userNotFound);
        done(error, session && session.user);
    }));
    return;
}
export const UserModule = (async () => {

    await preInitialize();
    const self = new GraphQLModule<IAuthConfig, ExpressContext, IGeexContext>({
        providers: () => [
            SessionStore,
            {
                provide: ioredis.default,
                useValue: new ioredis.default(environment.connections.redis),
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
                useFactory: (provider) => getModelForClass(User),
            },
            {
                provide: UserModuleConfigToken,
                useValue: environment.authConfig,
            },
            {
                provide: PasswordHasher,
                useFactory: (provider) => new PasswordHasher(environment.authConfig.tokenSecret),
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
