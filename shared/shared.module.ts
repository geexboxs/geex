import { GraphQLModule, GraphQLModuleOptions } from "@graphql-modules/core";
import { ApolloServer, IResolvers } from "apollo-server-express";
import { GeexServerConfigToken, LoggerConfigToken, AuthConfigToken } from "./tokens";
import { GeexLogger } from "./utils/logger";
import { Connection, createConnection } from "mongoose";
import express = require("express");
import { AuthModule } from "./authentication/auth.module";
import { LoggingModule } from "./logging/logging.module";
import { GeexContext, GeexServerConfig } from "./utils/abstractions";
import { ProviderScope, Injector } from "@graphql-modules/di";
import { environment } from "../environments/environment";
import { GlobalLoggingMiddleware } from "./global-logging.middleware";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";


let result = new GraphQLModule<GeexServerConfig, ExpressContext, GeexContext>({
    providers: ({ config }) => [
        {
            provide: GeexServerConfigToken,
            useValue: config
        }, {
            provide: LoggerConfigToken,
            useValue: config.loggerConfig
        }, {
            provide: AuthConfigToken,
            useValue: config.authConfig
        },
        GeexLogger,
        GlobalLoggingMiddleware,
    ],
    imports: [LoggingModule, AuthModule],
}, environment);
export const SharedModule = result;
