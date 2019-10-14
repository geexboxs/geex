import { GraphQLModule, GraphQLModuleOptions } from "@graphql-modules/core";
import { Injector, ProviderScope } from "@graphql-modules/di";
import { ApolloServer, IResolvers } from "apollo-server-express";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import express = require("express");
import { Connection, createConnection } from "mongoose";
import { environment } from "../environments/environment";
import { LoggingMiddleware } from "./audit-log/audit-log.middleware";
import { AuditLogModule } from "./audit-log/audit-log.module";
import { AuthModule } from "./auth/auth.module";
import { GlobalLoggingExtension } from "./extensions/global-logging.gql-extension";
import { RequestIdentityExtension } from "./extensions/request-identity.gql-extension";
import { AuthConfigToken, GeexServerConfigToken, LoggerConfigToken } from "./tokens";
import { IGeexContext, IGeexServerConfig } from "./utils/abstractions";
import { GeexLogger } from "./utils/logger";

const result = new GraphQLModule<IGeexServerConfig, ExpressContext, IGeexContext>({
    providers: ({ config }) => [
        {
            provide: GeexServerConfigToken,
            useValue: config,
        }, {
            provide: LoggerConfigToken,
            useValue: config.loggerConfig,
        }, {
            provide: AuthConfigToken,
            useValue: config.authConfig,
        },
        GeexLogger,
        LoggingMiddleware,
        RequestIdentityExtension,
        GlobalLoggingExtension,
    ],
    imports: [AuditLogModule, AuthModule],
}, environment);
export const SharedModule = result;
