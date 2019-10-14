import { GraphQLModule, GraphQLModuleOptions } from "@graphql-modules/core";
import { ApolloServer, IResolvers } from "apollo-server-express";
import { GeexGraphqlServerConfigToken } from "./tokens";
import { GeexLogger } from "./utils/logger";
import { Connection, createConnection } from "mongoose";
import express = require("express");
import { AuthModule } from "./authentication/auth.module";
import { LoggingModule } from "./logging/logging.module";
import { LoggerConfigToken } from "./logging/tokens";
import { GeexContext, GeexServerConfig } from "./utils/abstractions";
import { AuthConfigToken } from "./authentication/tokens";
import { ProviderScope, Injector } from "@graphql-modules/di";
import { environment } from "../environments/environment";
import { GlobalLoggingMiddleware } from "./global-logging.middleware";


let result = new GraphQLModule<GeexServerConfig, GeexContext, GeexContext, IResolvers>({
    providers: ({ config }) => [
        {
            provide: GeexGraphqlServerConfigToken,
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
