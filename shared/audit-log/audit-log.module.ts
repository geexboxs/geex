import { GraphQLModule } from "@graphql-modules/core";
import { Injector } from "@graphql-modules/di";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { printSchema } from "graphql";
import { buildSchemaSync } from "type-graphql";
import { inspect } from "util";
import { environment } from "../../environments/environment";
import { AuthModule } from "../auth/auth.module";
import { LoggerConfigToken } from "../tokens";
import { IGeexContext } from "../utils/abstractions";
import { GeexLogger, ILoggerConfig } from "../utils/logger";
import { LoggingMiddleware } from "./audit-log.middleware";
import { AuditLogResolver } from "./audit-log.resolver";
const resolvers: any = [AuditLogResolver];
export const AuditLogModule = new GraphQLModule<ILoggerConfig | undefined, ExpressContext, IGeexContext>({
    providers: [{
        provide: GeexLogger,
        useFactory: (injector: Injector) => new GeexLogger(injector.get(LoggerConfigToken)),
    }, LoggingMiddleware, ...resolvers],
    extraSchemas: () => [buildSchemaSync({
        resolvers,
        container: {
            get: (someClass, resolverData) => {
                return (resolverData.context as IGeexContext).injector.get(someClass);
            },
        },
    })],
    imports: [],
}, environment.loggerConfig);
