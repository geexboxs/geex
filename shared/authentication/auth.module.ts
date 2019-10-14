
import { Connection, createConnection } from "mongoose";
import { GraphQLModule, ModuleContext, GraphQLModuleOptions } from "@graphql-modules/core";
import { getModelForClass } from "@typegoose/typegoose";
import { User } from "./user.model";
import { GeexContext, GeexServerConfig } from "../utils/abstractions";
import { Strategy as LocalStrategy } from "passport-local";
import { PasswordHasher } from "./password-hasher";
import acl from "acl";
import { Db } from "mongodb";
import { AclToken, UserModelToken } from "./tokens";
import passport, { Passport } from "passport";
import { AuthResolver } from "./auth.resolver";
import { GeexServerConfigToken, AuthConfigToken } from "../tokens";
import { AuthMiddleware } from "./auth.middleware";
import { GraphQLResolveInfo, printSchema } from "graphql";
import { printSchemaWithDirectives } from "graphql-toolkit";
import { environment } from "../../environments/environment";
import { inspect } from "util";
import { ProviderScope } from "@graphql-modules/di";
import { buildSchema, buildSchemaSync } from "type-graphql";
import { GlobalLoggingMiddleware } from "../global-logging.middleware";
import { LoggingModule } from "../logging/logging.module";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";

const UserModel = getModelForClass(User);
export type AuthConfig = {
    tokenSecret: string;
}

export const AuthModule = new GraphQLModule<AuthConfig, ExpressContext, GeexContext>({
    defaultProviderScope: ProviderScope.Application,
    providers: (self) => [{
        provide: PasswordHasher,
        useFactory: injector => new PasswordHasher(injector.get<AuthConfig>(AuthConfigToken) && injector.get<AuthConfig>(AuthConfigToken).tokenSecret || "")
    }, {
        provide: Passport,
        useFactory: injector => passport.use("local", new LocalStrategy(
            function (username, password, done) {
                UserModel.findOne({ username: username }, function (err, user) {
                    if (err) { return done(err); }
                    if (!user) { return done(null, false); }
                    if (injector.get(PasswordHasher).hash(password) !== user.passwordHash) { return done(null, false); }
                    return done(null, user);
                });
            }
        ))
    }, {
        provide: AclToken,
        useFactory: injector => new acl(new acl.mongodbBackend(createConnection(injector.get<GeexServerConfig>(GeexServerConfigToken).connectionString).db, ""))
    }, {
        provide: UserModelToken,
        useValue: UserModel
    }, AuthResolver, AuthMiddleware],
    extraSchemas: () => [
        buildSchemaSync({
            resolvers: [AuthResolver],
            globalMiddlewares: [GlobalLoggingMiddleware],
            container: {
                get: (someClass, resolverData) => {
                    return (resolverData.context as GeexContext).injector.get(someClass);
                }
            },
        })
    ],
    imports: []
}, environment.authConfig);
// Creates resolvers, type definitions, and schema directives used by accounts-js
// export const AuthModule = AccountsModule.forRoot({
//     accountsServer,
// });
