import { GraphQLModuleOptions } from "@graphql-modules/core";
import { IResolvers } from "@kamilkisiela/graphql-tools";
import { application, urlencoded } from "express";
import { LoggerConfig } from "../logging/Logger";
import { GeexGraphqlServerModule, GeexGraphqlServerConfig } from "./geex-graphql-server.module";


declare module "express-serve-static-core" {
    interface Express {
        useGeexGraphql(this: Express, config: GeexGraphqlServerConfig): Promise<Express>;
    }
}

application["useGeexGraphql"] = async function useGeexGraphql(this, config: GeexGraphqlServerConfig) {
    const entryModule: GeexGraphqlServerModule = new GeexGraphqlServerModule(config);
    entryModule.addMongo();
    entryModule.addLogging();
    await entryModule.addAuth({ tokenSecret: "secret" });
    return entryModule.buildServer(this);
}
