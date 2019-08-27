import { DatabaseManager } from "@accounts/database-manager";

import AccountsPassword, { PasswordCreateUserType } from "@accounts/password";

import AccountsServer from "@accounts/server";
import MongoDBInterface from '@accounts/mongo';

import { AccountsModule } from "@accounts/boost";
import { Connection } from "mongoose";
import { AccountServerToken } from "./tokens";
import { buildSchemaSync } from "type-graphql";
import { RoleBasedAuthChecker } from "./role-based-auth-checker";
import { GraphQLModule, ModuleContext } from "@graphql-modules/core";
import { AccountsModuleConfig, AccountsRequest, AccountsModuleContext } from "@accounts/graphql-api";
import { User } from "@accounts/types";
import { IResolvers } from "@kamilkisiela/graphql-tools";

export type AuthConfig = {
    tokenSecret: string;
    userValidator?: ((user: PasswordCreateUserType) => PasswordCreateUserType | Promise<PasswordCreateUserType>)
}

export class AuthModule extends GraphQLModule<AccountsModuleConfig, AccountsRequest, AccountsModuleContext<User>, IResolvers<any, ModuleContext<AccountsModuleContext<User>>>> {
    /**
     *
     */
    constructor(config: AuthConfig, connection: Connection) {
        super();
        // Build a storage for storing users
        const userStorage = new MongoDBInterface(connection);
        // Create accounts server that holds a lower level of all accounts operations
        const accountsServer = new AccountsServer(
            {
                db: new DatabaseManager({
                    sessionStorage: userStorage,
                    userStorage,
                }),
                tokenSecret: config.tokenSecret,
            },
            {
                password: new AccountsPassword({
                    // This option is called when a new user create an account
                    // Inside we can apply our logic to validate the user fields
                    validateNewUser: config.userValidator
                }),
            }
        );
        this.forRoot({ accountsServer })
        this.selfProviders.push({
            provide: AccountServerToken,
            useValue: accountsServer
        })
        this.selfProviders.push({
            provide: RoleBasedAuthChecker,
            useValue: RoleBasedAuthChecker
        })
        this.extraSchemas.push(buildSchemaSync({
            authChecker: RoleBasedAuthChecker,
            resolvers: [this.resolvers],
            container: ({ ...args }) => {
                return this.injector.getSessionInjector(args.context);
            },
        }));
    }
}




// Creates resolvers, type definitions, and schema directives used by accounts-js
// export const AuthModule = AccountsModule.forRoot({
//     accountsServer,
// });
