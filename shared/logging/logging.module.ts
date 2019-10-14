import { GraphQLModule } from "@graphql-modules/core";
import { GeexLogger, LoggerConfig } from "../utils/logger";
import { Injector } from "@graphql-modules/di";
import { LoggerConfigToken } from "./tokens";
import { buildSchemaSync } from "type-graphql";
import { LogResolver } from "./log.resolver";
import { GeexContext } from "../utils/abstractions";
import { printSchema } from "graphql";
import { environment } from "../../environments/environment";
import { inspect } from "util";
import { AuthModule } from "../authentication/auth.module";
import { GlobalLoggingMiddleware } from "../global-logging.middleware";
const resolvers: any = [LogResolver];
export const LoggingModule = new GraphQLModule<LoggerConfig | undefined>({
    providers: [{
        provide: GeexLogger,
        useFactory: (injector: Injector) => new GeexLogger(injector.get(LoggerConfigToken))
    }, GlobalLoggingMiddleware, ...resolvers],
    extraSchemas: () => [buildSchemaSync({
        resolvers,
        globalMiddlewares: [GlobalLoggingMiddleware],
        container: {
            get: (someClass, resolverData) => {
                return (resolverData.context as GeexContext).injector.get(someClass);
            }
        }
    })],
    imports: []
}, environment.loggerConfig);
