import { Injector } from "@graphql-modules/di";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { TracingConfig } from "jaeger-client";
import { Request } from "apollo-env";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import { RequestStart } from "apollo-opentracing";

export interface IGeexContext {
    session: ExpressContext;
    user?: IUserContext;
    injector: Injector;
}

export interface IUserContext {
    id: string;
    username: string;
    roles: string[];
    emails: string[];
    phone: string;
}

type LogLevel = "debug" | "info" | "warn" | "error";

type LogTarget = "console" | "file" | "remote";


export interface IAuthConfig {
    tokenSecret: string;
}

export interface ILoggerConfig {
    /**
     * log target
     *
     * @type {LogTarget}
     * @default "console"
     * @memberof LoggerConfig
     */
    target?: LogTarget;
    /**
     * minimal level to log
     *
     * @type {LogLevel}
     * @default "info"
     * @memberof LoggerConfig
     */
    filterLevel?: LogLevel;
    consoleConfig?: {};
    fileConfig?: {};
    remoteConfig?: {};
    /**
     * metadata to be logged in every log entry
     *
     * @type {{}}
     * @memberof LoggerConfig
     */
    metadata?: {};
}

export interface IGeexServerConfig {
    hostname: string;
    port: number;
    connectionString: string;
    traceConfig: TracingConfig;
    loggerConfig: ILoggerConfig;
    userConfig: IAuthConfig;
}

export interface IGeexRequestStart<TContext = IGeexContext> extends RequestStart {
    context: IGeexContext;
    extensions?: any;
    queryString?: string;
    requestContext: GraphQLRequestContext,
}


export interface IGeexRequestEnd<TContext = IGeexContext> {
    graphqlResponse: GraphQLResponse;
    context: TContext;
}









declare module "type-graphql" {
    export function Ctx<T>(propertyName?: keyof T): ParameterDecorator;
}
