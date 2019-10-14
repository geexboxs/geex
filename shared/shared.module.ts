import { GraphQLModule, GraphQLModuleOptions } from "@graphql-modules/core";
import { ApolloServer, IResolvers } from "apollo-server-express";
import { GeexServerConfigToken, LoggerConfigToken, AuthConfigToken } from "./tokens";
import { GeexLogger } from "./utils/logger";
import { Connection, createConnection } from "mongoose";
import express = require("express");
import { AuthModule } from "./auth/auth.module";
import { AuditLogModule } from "./audit-log/audit-log.module";
import { GeexContext, GeexServerConfig } from "./utils/abstractions";
import { ProviderScope, Injector } from "@graphql-modules/di";
import { environment } from "../environments/environment";
import { LoggingMiddleware } from "./audit-log/audit-log.middleware";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { RequestIdentityExtension } from "./extensions/request-identity.gql-extension";
import { GlobalLoggingExtension } from "./extensions/global-logging.gql-extension";


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
        LoggingMiddleware,
        RequestIdentityExtension,
        GlobalLoggingExtension
    ],
    imports: [AuditLogModule, AuthModule],
}, environment);
export const SharedModule = result;
