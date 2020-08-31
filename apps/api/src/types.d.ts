import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { TracingConfig } from "jaeger-client";
import { Request } from "apollo-env";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import { RequestStart } from "apollo-opentracing";
import { Document } from "mongoose";
import { ObjectId } from "bson";
import { Injector } from "@nestjs/core/injector/injector";
import { LogLevel } from "@nestjs/common";
import { INestApplicationContext, ExecutionContext, IUserContext } from "@nestjs/common";

type LogTarget = "console" | "file" | "remote";

export interface IGeexRequestStart<TContext = ExecutionContext> extends RequestStart<TContext> {
  extensions?: any;
  queryString?: string;
}

export interface IGeexRequestEnd<TContext = ExecutionContext> {
  graphqlResponse: GraphQLResponse;
  context: TContext;
}
export type RequiredPartial<T, K extends keyof T> = Partial<T> & Required<Pick<T, K>>

declare global {
  namespace Express {
    interface AuthInfo { }
    interface User extends IUserContext {
    }

    interface Request {
      authInfo?: AuthInfo;
      user?: User;
    }
  }
}

export interface IAuthConfig {
  tokenSecret: string;
  expiresIn: string | number;
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
type ISmtpConfig = {
  secure: boolean;
  host: string;
  port: number;
  username: string;
  password: string;
  sendAs: {
    name: string;
    address: string;
  };
};
export interface IGeexServerConfig {
  hostname: string;
  port: number;
  connections: {
    mongo: string;
    redis: string;
    smtp?: ISmtpConfig;
  };
  traceConfig: TracingConfig;
  loggerConfig: ILoggerConfig;
  authConfig: IAuthConfig;
}
