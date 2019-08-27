import { GraphQLModule, GraphQLModuleOptions } from "@graphql-modules/core";
import { ApolloServer, IResolvers } from "apollo-server-express";
import { ExpressToken, ApolloToken } from "./tokens";
import { GeexLogger, LoggerConfig } from "../logging/Logger";
import { connect, Connection } from "mongoose";
import express = require("express");
import { GlobalLoggingExtension } from "../logging/global-logging.gql-extension";
import { RequestIdentityExtension } from "../logging/request-identity.gql-extension";
import { AuthModule, AuthConfig } from "../authentication/auth.module";
import { LoggingModule } from "../logging/logging.module";
import { LoggerConfigToken } from "../logging/tokens";
export type GeexGraphqlServerConfig = GraphQLModuleOptions<any, any, any, IResolvers<any, any>> & {
    connectionString: string;
    loggerConfig?: LoggerConfig;
};
export class GeexGraphqlServerModule extends GraphQLModule<GeexGraphqlServerConfig> {
    /**
     *
     */
    constructor(config: GeexGraphqlServerConfig) {
        super(config);
        this.forRoot(config)
    }
    addMongo(): this {
        this.selfProviders.push({
            provide: Connection,
            useFactory: async () => (await connect(this.config.connectionString, { useNewUrlParser: true })).connection
        });
        return this;
    }
    addLogging(): this {
        this.selfProviders.push({
            provide: LoggerConfigToken,
            useValue: this.config.loggerConfig
        });
        this.selfImports.push(LoggingModule);
        return this;
    }
    async addAuth(authConfig: AuthConfig): Promise<this> {
        let authModule = new AuthModule(authConfig, (await connect(this.config.connectionString, { useNewUrlParser: true })).connection);
        this.selfImports.push(authModule);
        this.selfExtraSchemas.push(...authModule.extraSchemas)
        return this;
    }
    buildServer(app: express.Application): express.Application {
        // 根据entryModule生成 graphql server
        const apollo = new ApolloServer({
            // typeDefs: this.typeDefs,
            // schema: this.schema,
            modules: [this],
            // // @ts-ignore
            // schemaDirectives: {
            //     // In order for the `@auth` directive to work
            //     ...this.schemaDirectives,
            // },
            context: ({ req, res }) => {
                return this.context({ req });
            },
            // resolvers: this.resolvers,
            uploads: {
                maxFileSize: 100000000,
                maxFiles: 10,
            },
            formatError: error => {
                LoggingModule.injector.get(GeexLogger).error(error);
                return error;
            },
            extensions: [() => LoggingModule.injector.get(RequestIdentityExtension),
            () => LoggingModule.injector.get(GlobalLoggingExtension)]
        });
        app.use(express.json());
        // 将 graphql server 挂载到 express
        apollo.applyMiddleware({ app: app });
        // 注入 graphql server 到 this
        this.selfProviders.push({
            provide: ApolloToken,
            useValue: apollo
        });
        this.selfProviders.push({
            provide: ExpressToken,
            useValue: app
        });
        return app;
    }
}
