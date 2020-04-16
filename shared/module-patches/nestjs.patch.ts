import { INestApplicationContext, IUserContext, Type, Abstract } from "@nestjs/common";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { ModelBase } from "../utils/model-base";
import { NestApplication } from "@nestjs/core";



type IPassportContext = {
    authenticate: (policyName: "local" | "jwt", options: { username: string, password: string }) => { user: IUserContext }
    getUser: () => IUserContext;
    isAuthenticated: () => any
    isUnauthenticated: () => any
    login: (user: IUserContext, options?: any) => PromiseLike<void>
    logout: () => any
}

declare module "@nestjs/common" {

    export interface IUserContext {
        userId: string;
        username: string;
        roles: string[];
        scopes: string[];
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
    export class NestApplication {
        get<TInput = any, TResult = TInput>(typeOrToken: Type<TInput> | Abstract<TInput> | string | symbol, options?: {
            strict: boolean;
        }): TResult;
        getModel<T extends ModelBase<T>>(ctorOrName: Type<T> | string): ModelType<T>;
    }
    export interface INestApplication {
        getModel<T extends ModelBase<T>>(ctorOrName: Type<T> | string): ModelType<T>;
        get<TInput = any, TResult = TInput>(typeOrToken: Type<TInput> | Abstract<TInput> | string | symbol, options?: {
            strict: boolean;
        }): TResult;
    }
}

NestApplication.prototype.getModel = function <T>(this: NestApplication, ctorOrName: Type<T> | string) {
    let modelName;
    if (typeof ctorOrName == "string") {
        modelName = ctorOrName;
    }
    else {
        modelName = ctorOrName.name;
    }
    return this.get(modelName + "Model");
};
