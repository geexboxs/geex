import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { TracingConfig } from "jaeger-client";
import { Request } from "apollo-env";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import { RequestStart } from "apollo-opentracing";
import { Document } from "mongoose";
import { ObjectId } from "bson";
import { Injector } from "@nestjs/core/injector/injector";
import { INestApplicationContext, ExecutionContext, IUserContext } from "@nestjs/common";

type LogLevel = "debug" | "info" | "warn" | "error";

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
