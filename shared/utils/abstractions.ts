import { Injector } from "@graphql-modules/di";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { User } from "../../app/user/user.model";
import { IAuthConfig } from "../auth/auth.module";
import { ILoggerConfig } from "./logger";

export interface IGeexContext {
    session: ExpressContext;
    user?: User;
    injector: Injector;
}

export interface IGeexServerConfig {
    connectionString: string;
    loggerConfig?: ILoggerConfig;
    authConfig?: IAuthConfig;
}
