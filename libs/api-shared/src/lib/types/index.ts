import { ModelType } from "@typegoose/typegoose/lib/types";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { TracingConfig } from "jaeger-client";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import { RequestStart } from "apollo-opentracing";
import { LogLevel, Type } from "@nestjs/common";
import { InjectModel } from "@nestjs/mongoose";
import { INestApplicationContext, ExecutionContext, IUserContext, Abstract } from "@nestjs/common";
import { ModelBase } from '@geex/api-shared';
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

declare global {
  // tslint:disable-next-line: interface-name
  interface Date {
    add: (value: { years?: number; months?: number; weeks?: number; days?: number; hours?: number; minutes?: number; seconds?: number; milliseconds?: number; }) => Date;
    format(format: string): string;
    epoch(): number;
  }
  // tslint:disable-next-line: interface-name
  interface String {
    isNotEmpty(): boolean;
  }
}


export interface AuditLog {
  content: string;
}
export interface IGeexRequestStart<TContext = ExecutionContext> extends RequestStart<TContext> {
  extensions?: any;
  queryString?: string;
}

export interface IGeexRequestEnd<TContext = ExecutionContext> {
  graphqlResponse: GraphQLResponse;
  context: TContext;
}
export type LogTarget = "console" | "file" | "remote";
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
export type ISmtpConfig = {
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

declare module "@nestjs/common" {

  export interface IPassportContext {
    authenticate: (policyName: "local" | "jwt", options: { username: string, password: string }) => { user: IUserContext }
    getUser: () => IUserContext;
    isAuthenticated: () => any
    isUnauthenticated: () => any
    login: (user: IUserContext, options?: any) => PromiseLike<void>
    logout: () => any
  }

  export interface IUserContext {
    userId: string;
    username: string;
    roles: string[];
    email: string;
    phone: string;
    avatarUrl: string;
  }

  export interface ExecutionContext extends IPassportContext, ExpressContext {
    injector: INestApplicationContext;
  }
  export interface INestApplication {
    getModel<T extends ModelBase<T>>(ctorOrName: Type<T> | string): ModelType<T>
  }
}

declare module "@nestjs/core" {
  export interface INestApplication {
    getModel<T extends ModelBase<T>>(ctorOrName: Type<T> | string): ModelType<T>;
    get<TInput = any, TResult = TInput>(typeOrToken: Type<TInput> | Abstract<TInput> | string | symbol, options?: {
      strict: boolean;
    }): TResult;
  }
}
