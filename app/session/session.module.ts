import { ClassType, buildSchemaSync } from "type-graphql";
import { SessionResolver } from "./session.resolver";
import { GraphQLModule } from "@graphql-modules/core";
import { Passport } from "passport";
import passport = require("passport");
import json5 = require("json5");
import { ReturnModelType, getModelForClass } from "@typegoose/typegoose";
import { User } from "../account/models/user.model";
import { UserModelToken, AuthConfigToken } from "../../shared/tokens";
import { PasswordHasher } from "../account/utils/password-hasher";
import { I18N } from "../../shared/utils/i18n";
import { IUserContext, IAuthConfig, IGeexContext } from "../../types";
import { appConfig } from "../../configs/app-config";
import { SessionStore } from "./models/session.model";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { Jwt } from "../../shared/utils/jwt";
import { EmailSender } from "../../shared/utils/email-sender";
import { GraphQLLocalStrategy } from "graphql-passport";
import { Strategy as JwtStrategy, ExtractJwt } from "passport-jwt";
import { RbacAuthChecker } from "../../shared/utils/rbac-auth-checker";
import ioredis = require("ioredis");

const resolvers: [ClassType] = [SessionResolver];

async function preInitialize() {
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
        secretOrKey: appConfig.authConfig.tokenSecret,
        issuer: appConfig.hostname,
        audience: "accessToken",
    }, async (jwt_payload, done) => {
        const userId = jwt_payload && jwt_payload.sub;
        if (userId === undefined) {
            done(undefined, undefined);
        }
        const sessionStore = self.injector.get(SessionStore);
        const session = await sessionStore.get(userId);
        if (session === null) {
            await sessionStore.del(userId);
            throw new Error(I18N.message.sessionNotFound);
        } else {
            done(undefined, session && session.user);
        }
    }));
    return;
}
export const SessionModule = (async () => {
    await preInitialize();
    const self = new GraphQLModule<IAuthConfig, ExpressContext, IGeexContext>({
        providers: () => [
            SessionStore,
            {
                provide: ioredis,
                useValue: new ioredis(appConfig.connections.redis),
            },
            {
                provide: Jwt,
                useValue: new Jwt(appConfig.authConfig.tokenSecret, `${appConfig.hostname}`),
            },
            {
                provide: Passport,
                useValue: passport.initialize(),
            },
            {
                provide: UserModelToken,
                useFactory: (injector) => {
                    return getModelForClass(User);
                },
            },
            {
                provide: AuthConfigToken,
                useValue: appConfig.authConfig,
            },
            {
                provide: PasswordHasher,
                useFactory: (injector) => new PasswordHasher(appConfig.authConfig.tokenSecret),
            },
            {
                provide: EmailSender,
                useValue: appConfig.connections.smtp && new EmailSender(appConfig.connections.smtp.sendAs, {
                    secure: appConfig.connections.smtp.secure,
                    auth: {
                        user: appConfig.connections.smtp.username,
                        pass: appConfig.connections.smtp.password,
                    },
                    host: appConfig.connections.smtp.host,
                    port: appConfig.connections.smtp.port,
                }),
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
    }, appConfig.authConfig);

    await postInitialize(self);
    return self;
})();
