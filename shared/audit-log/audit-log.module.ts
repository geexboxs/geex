import { GraphQLModule } from "@graphql-modules/core";
import { Injector } from "@graphql-modules/di";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { printSchema } from "graphql";
import { buildSchemaSync } from "type-graphql";
import { inspect } from "util";
import { environment } from "../../environments/environment";
import { AuthModule } from "../auth/auth.module";
import { LoggerConfigToken } from "../tokens";
import { GeexContext } from "../utils/abstractions";
import { GeexLogger, LoggerConfig } from "../utils/logger";
import { LoggingMiddleware } from "./audit-log.middleware";
import { AuditLogResolver } from "./audit-log.resolver";
const resolvers: any = [AuditLogResolver];
export const AuditLogModule = new GraphQLModule<LoggerConfig | undefined, ExpressContext, GeexContext>({
    providers: [{
        provide: GeexLogger,
        useFactory: (injector: Injector) => new GeexLogger(injector.get(LoggerConfigToken)),
    }, LoggingMiddleware, ...resolvers],
    extraSchemas: () => [buildSchemaSync({
        resolvers,
        container: {
            get: (someClass, resolverData) => {
                return (resolverData.context as GeexContext).injector.get(someClass);
            },
        },
    })],
    imports: [],
}, environment.loggerConfig);
