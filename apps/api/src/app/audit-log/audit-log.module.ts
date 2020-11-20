// import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
// import { buildSchemaSync, ClassType } from "type-graphql";
// import { environment } from '@geex/api/app/app_config';
// import { AuditLogResolver } from "./audit-log.resolver";
// import { ExecutionContext } from "@nestjs/common";
// import { GraphQLModule } from '@nestjs/graphql';
// import { ILoggerConfig, GeexLogger, RbacAuthChecker, LoggerConfigToken } from '@geex/api-shared';

// const resolvers: [ClassType] = [AuditLogResolver];
// export const AuditLogModule = new GraphQLModule<ILoggerConfig | undefined, ExpressContext, ExecutionContext>({
//     providers: [, {
//         provide: LoggerConfigToken,
//         useValue: environment.loggerConfig,
//     }, ...resolvers],
//     extraSchemas: () => [buildSchemaSync({
//         resolvers,
//         container: {
//             get: (someClass, resolverData) => {
//                 return (resolverData.context as ExecutionContext).injector.get(someClass);
//             },
//         },
//         authChecker: RbacAuthChecker,
//     })],
//     imports: [],
// }, environment.loggerConfig);
