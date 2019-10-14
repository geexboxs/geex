import { GraphQLModule } from "@graphql-modules/core";
import { GeexLogger, LoggerConfig } from "../utils/logger";
import { Injector } from "@graphql-modules/di";
import { buildSchemaSync } from "type-graphql";
import { AuditLogResolver } from "./audit-log.resolver";
import { GeexContext } from "../utils/abstractions";
import { printSchema } from "graphql";
import { environment } from "../../environments/environment";
import { inspect } from "util";
import { AuthModule } from "../auth/auth.module";
import { LoggingMiddleware } from "./audit-log.middleware";
import { LoggerConfigToken } from "../tokens";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
const resolvers: any = [AuditLogResolver];
export const AuditLogModule = new GraphQLModule<LoggerConfig | undefined, ExpressContext, GeexContext>({
    providers: [{
        provide: GeexLogger,
        useFactory: (injector: Injector) => new GeexLogger(injector.get(LoggerConfigToken))
    }, LoggingMiddleware, ...resolvers],
    extraSchemas: () => [buildSchemaSync({
        resolvers,
        container: {
            get: (someClass, resolverData) => {
                return (resolverData.context as GeexContext).injector.get(someClass);
            }
        }
    })],
    imports: []
}, environment.loggerConfig);
