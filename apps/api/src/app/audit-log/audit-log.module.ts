import { GraphQLModule } from "@graphql-modules/core";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { buildSchemaSync, ClassType } from "type-graphql";
import { environments } from "../../configs/app-config";
import { AuditLogResolver } from "./audit-log.resolver";
import { GeexLogger } from "../../shared/utils/logger";
import { LoggerConfigToken } from "../../shared/tokens";
import { RbacAuthChecker } from "../../shared/utils/rbac-auth-checker";
import { ILoggerConfig } from "../../configs/types";
import { ExecutionContext } from "@nestjs/common";

const resolvers: [ClassType] = [AuditLogResolver];
export const AuditLogModule = new GraphQLModule<ILoggerConfig | undefined, ExpressContext, ExecutionContext>({
    providers: [GeexLogger, {
        provide: LoggerConfigToken,
        useValue: environments.loggerConfig,
    }, ...resolvers],
    extraSchemas: () => [buildSchemaSync({
        resolvers,
        container: {
            get: (someClass, resolverData) => {
                return (resolverData.context as ExecutionContext).injector.get(someClass);
            },
        },
        authChecker: RbacAuthChecker,
    })],
    imports: [],
}, environments.loggerConfig);
