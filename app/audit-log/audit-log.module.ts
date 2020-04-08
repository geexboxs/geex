import { GraphQLModule } from "@graphql-modules/core";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { buildSchemaSync, ClassType } from "type-graphql";
import { appConfig } from "../../configs/app-config";
import { IGeexContext } from "../../types";
import { AuditLogResolver } from "./audit-log.resolver";
import { GeexLogger } from "../../shared/utils/logger";
import { LoggerConfigToken } from "../../shared/tokens";
import { RbacAuthChecker } from "../../shared/utils/rbac-auth-checker";
import { ILoggerConfig } from "../../configs/types";

const resolvers: [ClassType] = [AuditLogResolver];
export const AuditLogModule = new GraphQLModule<ILoggerConfig | undefined, ExpressContext, IGeexContext>({
    providers: [GeexLogger, {
        provide: LoggerConfigToken,
        useValue: appConfig.loggerConfig,
    }, ...resolvers],
    extraSchemas: () => [buildSchemaSync({
        resolvers,
        container: {
            get: (someClass, resolverData) => {
                return (resolverData.context as IGeexContext).injector.get(someClass);
            },
        },
        authChecker: RbacAuthChecker,
    })],
    imports: [],
}, appConfig.loggerConfig);
