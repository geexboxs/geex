import { GraphQLModule, GraphQLModuleOptions } from "@graphql-modules/core";
import { IResolvers } from "@kamilkisiela/graphql-tools";
import { application } from "express";
import accountsBoost from "@accounts/boost";
import { ApolloServer } from "apollo-server-express";
import bodyParser = require("body-parser");
import { ExpressToken, ApolloToken } from "../tokens";
import { buildSchemaSync } from "type-graphql";
import { RoleBasedAuthChecker } from "../../shared/authentication/role-based-auth-checker";
import { globalLoggingMiddleware } from "../../shared/logging/globalLogging.middleware";
import { DatabaseManager } from '@accounts/database-manager';
import { AccountsModule } from '@accounts/graphql-api';
import MongoDBInterface from '@accounts/mongo';
import { AccountsPassword } from '@accounts/password';
import { AccountsServer } from '@accounts/server';
import mongoose from 'mongoose';
type GeexGraphqlServerConfig = GraphQLModuleOptions<any, any, any, IResolvers<any, any>> & {
    useAccounts?: boolean;
};

declare module "express-serve-static-core" {
    interface Express {
        useGeexGraphql(this: Express, config: GeexGraphqlServerConfig): Promise<Express>;
    }
}

application["useGeexGraphql"] = async function useGeexGraphql(this, config: GeexGraphqlServerConfig) {
    this.use(bodyParser())
        .use(globalLoggingMiddleware)
    config.providers = config.providers === undefined ? [] : config.providers as []
    config.imports = config.imports === undefined ? [] : config.imports as []

    config.providers.push({
        provide: ExpressToken,
        useValue: this
    })
    const entryModule = new GraphQLModule(config);

    //其他的模块初始化逻辑


    // 在其他模块之上统一加上auth

    const mongoConn = mongoose.connection;

    // Build a storage for storing users
    const userStorage = new MongoDBInterface(mongoConn);
    // Create database manager (create user, find users, sessions etc) for accounts-js
    const accountsDb = new DatabaseManager({
        sessionStorage: userStorage,
        userStorage,
    });

    const accountsPassword = new AccountsPassword({
        // This option is called when a new user create an account
        // Inside we can apply our logic to validate the user fields
        validateNewUser: user => {
            // For example we can allow only some kind of emails
            if (!user.email || user.email.endsWith('.xyz')) {
                throw new Error('Invalid email');
            }
            return user;
        },
    });

    // Create accounts server that holds a lower level of all accounts operations
    const accountsServer = new AccountsServer(
        {
            db: accountsDb, tokenSecret: 'secret',
        },
        {
            password: accountsPassword,
        }
    );

    // Creates resolvers, type definitions, and schema directives used by accounts-js
    const accountsGraphQL = AccountsModule.forRoot({
        
        accountsServer,
    });
    // const accounts = (await accountsBoost({
    //     tokenSecret: 'terrible secret',
    //     // micro: true, // setting micro to true will instruct `@accounts/boost` to only verify access tokens without any additional session logic
    // }));
    // const accountsModule = accounts.graphql();
    config.imports.push(accountsGraphQL)
    config.extraSchemas = [buildSchemaSync({
        authChecker: RoleBasedAuthChecker,
        resolvers: [entryModule.resolvers],
        container: ({ ...args }) => {
            return entryModule.injector.getSessionInjector(args.context)
        },
    })]

    // 根据entryModule生成 graphql server
    const apollo = new ApolloServer({
        typeDefs: entryModule.typeDefs,
        schema: entryModule.schema,
        // @ts-ignore
        schemaDirectives: {
            // In order for the `@auth` directive to work
            ...entryModule.schemaDirectives,
        },
        context: ({ req }) => entryModule.context({ req }),
        resolvers: entryModule.resolvers,
    });
    // 将 graphql server 挂载到 express
    apollo.applyMiddleware({ app: this })

    // 注入 graphql server 到 entryModule
    entryModule.selfProviders.push({
        provide: ApolloToken,
        useValue: apollo
    })

    return this;
}
