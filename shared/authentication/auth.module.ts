
import { Connection } from "mongoose";
import { buildSchemaSync } from "type-graphql";
import { DefaultAuthChecker } from "./role-based-auth-checker";
import { GraphQLModule, ModuleContext } from "@graphql-modules/core";
import { IResolvers } from "@kamilkisiela/graphql-tools";
import { getModelForClass } from "@typegoose/typegoose";
import { User } from "./user.model";
import { GeexContext } from "../context.interface";
import { Strategy as LocalStrategy } from "passport-local";
import { PasswordHasher } from "./password-hasher";
import acl from "acl";
import { Db } from "mongodb";
import { AclToken } from "./tokens";
import passport from "passport";
import { AuthResolver } from "./auth.resolver";

const UserModel = getModelForClass(User);
export const UserModelToken = Symbol("UserModel");

export type AuthConfig = {
    tokenSecret: string;
}

export class AuthModule extends GraphQLModule<AuthConfig, GeexContext, GeexContext> {
    /**
     *
     */
    constructor(config: AuthConfig, connection: Connection) {
        super();
        const passwordHasher = new PasswordHasher(config.tokenSecret);
        const aclInstance = new acl(new acl.mongodbBackend(connection.db, ""));
        this.selfProviders.push({
            provide: PasswordHasher,
            useValue: passwordHasher
        })
        this.selfProviders.push({
            provide: AclToken,
            useValue: aclInstance
        })
        this.selfProviders.push({
            provide: UserModelToken,
            useValue: UserModel
        })
        passport.use("local", new LocalStrategy(
            function (username, password, done) {
                UserModel.findOne({ username: username }, function (err, user) {
                    if (err) { return done(err); }
                    if (!user) { return done(null, false); }
                    if (passwordHasher.hash(password) !== user.passwordHash) { return done(null, false); }
                    return done(null, user);
                });
            }
        ));

        this.selfProviders.push({
            provide: DefaultAuthChecker,
            useValue: DefaultAuthChecker
        })
        let schema = buildSchemaSync({
            authChecker: DefaultAuthChecker,
            resolvers: [AuthResolver],
            container: ({ ...args }) => {
                return this.injector.getSessionInjector(args.context);
            },
        });
        this.extraSchemas.push(schema);
    }
}




// Creates resolvers, type definitions, and schema directives used by accounts-js
// export const AuthModule = AccountsModule.forRoot({
//     accountsServer,
// });
