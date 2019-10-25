import { GraphQLModule } from "@graphql-modules/core";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { buildSchemaSync, ClassType } from "type-graphql";
import { environment } from "../../environments/environment";
import { IGeexContext, ILoggerConfig } from "../../types";
import { AuditLogResolver } from "./audit-log.resolver";
import { GeexLogger } from "../../shared/utils/logger";
import { LoggerConfigToken } from "../../shared/tokens";
import { RbacAuthChecker } from "../../shared/utils/rbac-auth-checker";

const resolvers: [ClassType] = [AuditLogResolver];
export const AuditLogModule = new GraphQLModule<ILoggerConfig | undefined, ExpressContext, IGeexContext>({
    providers: [GeexLogger, {
        provide: LoggerConfigToken,
        useValue: environment.loggerConfig,
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
}, environment.loggerConfig);
