import { GraphQLModule } from '@graphql-modules/core';
import { UserResolver } from './user.resolver';
import { buildSchemaSync } from 'type-graphql';
import { DomainModule } from '../../domain/domain.module';
import { RoleBasedAuthChecker } from "../../shared/authentication/role-based-auth-checker";
import { GeexContext } from '../../shared/context.interface';
import { Request, request } from 'express';
import { LogInfo } from "../custom-middlewares/middlewares/LogInfo";
// import { StudentModelToken, StudentModel } from '../../domain/models/student.model';
export const UserModule: any = new GraphQLModule({
    extraSchemas: [
        buildSchemaSync({
            authChecker: RoleBasedAuthChecker,
            resolvers: [UserResolver],
            container: ({ ...args }) => {
                return UserModule.injector.getSessionInjector(args.context)
            },
            // container: ({ context }) => context.injector

        })
    ],
    imports: [DomainModule]
});
