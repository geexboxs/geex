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

export interface IGeexContext  {
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

declare module "type-graphql" {
    export function Ctx<T extends IGeexContext = IGeexContext>(propertyName?: keyof T): ParameterDecorator;
}

declare global {
    namespace Express {
        // tslint:disable-next-line:no-empty-interface
        interface AuthInfo { }
        // tslint:disable-next-line:no-empty-interface
        interface User extends IUserContext { }

        interface Request {
            authInfo?: AuthInfo;
            user?: User;

            // These declarations are merged into express's Request type
            login(user: User, done: (err: any) => void): void;
            login(user: User, options: any, done: (err: any) => void): void;
            logIn(user: User, done: (err: any) => void): void;
            logIn(user: User, options: any, done: (err: any) => void): void;

            logout(): void;
            logOut(): void;

            isAuthenticated(): boolean;
            isUnauthenticated(): boolean;
        }
    }
}

type ModelFieldResolver<T, TKey extends keyof T = any> = (this: T, ...params: T[TKey] extends (...args: any) => any ? Parameters<T[TKey]> : never) => T[TKey];



/** fields not in base class of mongoose Document. */
// export type GeexEntityIntersection<T = any> = Partial<Omit<T, keyof Document>>;
// export type GeexPrimitive = string | number | bigint | boolean | symbol | String | Number | Date | Boolean | BigInt | Symbol | ObjectId | undefined;

// export type PrimitiveIntersection<T> = {
//     [key in keyof T]: T[key] extends GeexPrimitive ? key : never
// }[keyof T];

// /** concrete fields that can be used in doc query. */
// export type QueryableIntersection<T> = Omit<{
//     [key in PrimitiveIntersection<T>]?: T[key]
// }, keyof Omit<Document, keyof { id }>>
