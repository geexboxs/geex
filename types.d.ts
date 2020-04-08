import { Injector } from "@graphql-modules/di";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { TracingConfig } from "jaeger-client";
import { Request } from "apollo-env";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import { RequestStart } from "apollo-opentracing";
import { Document } from "mongoose";
import { ObjectId } from "bson";

type ISubscriptionContext = {
    connection?: any;
    payload?: any;
};
type IPassportContext = {
    authenticate: (policyName: "local" | "jwt", options: { username: string, password: string }) => { user: IUserContext }
    getUser: () => IUserContext;
    isAuthenticated: () => any
    isUnauthenticated: () => any
    login: (user: IUserContext, options?: any) => PromiseLike<void>
    logout: () => any
}

export interface IGeexContext {
    session: ExpressContext & ISubscriptionContext & IPassportContext;
    injector: Injector;
}

export interface IUserContext {
    id: string;
    username: string;
    roles: string[];
    email: string;
    phone: string;
    avatarUrl: string;
}

type LogLevel = "debug" | "info" | "warn" | "error";

type LogTarget = "console" | "file" | "remote";

export interface IGeexRequestStart<TContext = IGeexContext> extends RequestStart<TContext> {
    extensions?: any;
    queryString?: string;
}


export interface IGeexRequestEnd<TContext = IGeexContext> {
    graphqlResponse: GraphQLResponse;
    context: TContext;
}
export type RequiredPartial<T, K extends keyof T> = Partial<T> & Required<Pick<T, K>>
