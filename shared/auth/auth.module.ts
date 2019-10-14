
import { GraphQLModule, GraphQLModuleOptions, ModuleContext } from "@graphql-modules/core";
import { ProviderScope } from "@graphql-modules/di";
import { getModelForClass } from "@typegoose/typegoose";
import acl from "acl";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { GraphQLResolveInfo, printSchema } from "graphql";
import { printSchemaWithDirectives } from "graphql-toolkit";
import { Db } from "mongodb";
import { Connection, createConnection } from "mongoose";
import passport, { Passport } from "passport";
import { Strategy as LocalStrategy } from "passport-local";
import { buildSchema, buildSchemaSync } from "type-graphql";
import { inspect } from "util";
import { environment } from "../../environments/environment";
import { LoggingMiddleware } from "../audit-log/audit-log.middleware";
import { AuditLogModule } from "../audit-log/audit-log.module";
import { AuthConfigToken, GeexServerConfigToken } from "../tokens";
import { IGeexContext, IGeexServerConfig } from "../utils/abstractions";
import { AuthMiddleware } from "./auth.middleware";
import { AuthResolver } from "./auth.resolver";
import { AclToken, UserModelToken } from "./tokens";
import { User } from "./user.model";
import { PasswordHasher } from "./utils/password-hasher";

const UserModel = getModelForClass(User);
export interface IAuthConfig {
    tokenSecret: string;
}

export const AuthModule = new GraphQLModule<IAuthConfig, ExpressContext, IGeexContext>({
    defaultProviderScope: ProviderScope.Application,
    providers: (self) => [{
        provide: PasswordHasher,
        useFactory: (injector) => new PasswordHasher(injector.get<IAuthConfig>(AuthConfigToken) && injector.get<IAuthConfig>(AuthConfigToken).tokenSecret || ""),
    }, {
        provide: Passport,
        useFactory: (injector) => passport.use("local", new LocalStrategy(
            (username, password, done) => {
                UserModel.findOne({ username }, (err, user) => {
                    if (err) { return done(err); }
                    if (!user) { return done(null, false); }
                    if (injector.get(PasswordHasher).hash(password) !== user.passwordHash) { return done(null, false); }
                    return done(null, user);
                });
            },
        )),
    }, {
        provide: AclToken,
        useFactory: (injector) => new acl(new acl.mongodbBackend(createConnection(injector.get<IGeexServerConfig>(GeexServerConfigToken).connectionString).db, "")),
    }, {
        provide: UserModelToken,
        useValue: UserModel,
    }, AuthResolver, AuthMiddleware],
    extraSchemas: () => [
        buildSchemaSync({
            resolvers: [AuthResolver],
            container: {
                get: (someClass, resolverData) => {
                    return (resolverData.context as IGeexContext).injector.get(someClass);
                },
            },
        }),
    ],
    imports: [],
}, environment.authConfig);
// Creates resolvers, type definitions, and schema directives used by accounts-js
// export const AuthModule = AccountsModule.forRoot({
//     accountsServer,
// });
