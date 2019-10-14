import { Injector } from "@graphql-modules/di";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { User } from "../../app/user/user.model";
import { AuthConfig } from "../auth/auth.module";
import { LoggerConfig } from "./logger";

export interface GeexContext {
    session: ExpressContext;
    user?: User;
    injector: Injector;
}

export interface GeexServerConfig {
    connectionString: string;
    loggerConfig?: LoggerConfig;
    authConfig?: AuthConfig;
}
