import { GraphQLModule, GraphQLModuleOptions } from "@graphql-modules/core";
import { IResolvers } from "@kamilkisiela/graphql-tools";
import { application, urlencoded } from "express";
import { ApolloServer } from "apollo-server-express";
import { ExpressToken, ApolloToken, AccountsToken } from "../server/tokens";
import { buildSchemaSync } from "type-graphql";
import { RoleBasedAuthChecker } from "../shared/authentication/role-based-auth-checker";
import { globalLoggingMiddleware } from "../shared/logging/globalLogging.middleware";
import { DatabaseManager } from '@accounts/database-manager';
import { AccountsModule } from '@accounts/graphql-api';
import MongoDBInterface from '@accounts/mongo';
import { AccountsPassword } from '@accounts/password';
import { AccountsServer } from '@accounts/server';
import { LoggerConfig, GeexLogger } from "../shared/logging/Logger";
import { connect } from "mongoose";
import express = require("express");

type GeexGraphqlServerConfig = {
    entryModuleOption: GraphQLModuleOptions<any, any, any, IResolvers<any, any>>;
    useAccounts?: boolean;
    connectionString: string;
    loggerConfig?: LoggerConfig;
};

declare module "express-serve-static-core" {
    interface Express {
        useGeexGraphql(this: Express, config: GeexGraphqlServerConfig): Promise<Express>;
    }
}

application["useGeexGraphql"] = async function useGeexGraphql(this, config: GeexGraphqlServerConfig) {
    let connection = (await connect(config.connectionString, { useNewUrlParser: true })).connection;
    this.use(express.json())
        .use(globalLoggingMiddleware);
    config.entryModuleOption.providers = config.entryModuleOption.providers === undefined ? [] : config.entryModuleOption.providers as []
    config.entryModuleOption.imports = config.entryModuleOption.imports === undefined ? [] : config.entryModuleOption.imports as []

    config.entryModuleOption.providers.push({
        provide: ExpressToken,
        useValue: this
    })
    config.entryModuleOption.providers.push({
        provide: GeexLogger,
        useValue: new GeexLogger(config.loggerConfig)
    })
    const entryModule = new GraphQLModule(config.entryModuleOption);

    //其他的模块初始化逻辑

    // 在其他模块之上统一加上auth


    // Build a storage for storing users
    const userStorage = new MongoDBInterface(connection);
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
            db: accountsDb,
            tokenSecret: 'secret',
        },
        {
            password: accountsPassword,
        }
    );
    // Creates resolvers, type definitions, and schema directives used by accounts-js
    const accountsGraphQL = AccountsModule.forRoot({
        accountsServer,
    });

    entryModule.selfProviders.push({
        provide: AccountsToken,
        useValue: accountsServer
    })

    config.entryModuleOption.imports.push(accountsGraphQL)
    config.entryModuleOption.extraSchemas = [buildSchemaSync({
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
        uploads: {
            maxFileSize: 100000000, // 100 MB
            maxFiles: 10
        }
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
