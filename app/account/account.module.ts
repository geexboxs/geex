
import { GraphQLModule } from "@graphql-modules/core";
import { ProviderScope, Type } from "@graphql-modules/di";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { createConnection } from "mongoose";
import { appConfig } from "../../configs/app-config";
import { IGeexContext, IGeexServerConfig, IAuthConfig, IUserContext } from "../../types";
import { GeexServerConfigToken, UserModelToken, AuthConfigToken } from "../../shared/tokens";
import { Model, Enforcer, newEnforcer } from "casbin";
import MongooseAdapter = require("@elastic.io/casbin-mongoose-adapter");
import { CasbinModel } from "./utils/casbin-model";
import { AccountResolver } from "./account.resolver";
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
import { SessionStore } from "../session/models/session.model";
import json5 = require("json5");
import { Strategy as JwtStrategy, ExtractJwt } from "passport-jwt";
import { RbacAuthChecker } from "../../shared/utils/rbac-auth-checker";
import { EmailSender } from "../../shared/utils/email-sender";

const resolvers: [ClassType] = [AccountResolver];
let enforcer: Enforcer;
async function preInitialize() {
    const adapter = await MongooseAdapter.newAdapter(appConfig.connections.mongo);
    enforcer = await newEnforcer(CasbinModel, adapter);
    enforcer.enableAutoSave(true);
    await enforcer.loadPolicy();
    return;
}
async function postInitialize(self: GraphQLModule) {
    return;
}
export const AccountModule = (async () => {
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
    }, appConfig.authConfig);

    await postInitialize(self);
    return self;
})();
