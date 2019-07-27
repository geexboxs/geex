import { GraphQLModule } from '@graphql-modules/core';
import { UserResolver } from './user.resolver';
import { buildSchemaSync } from 'type-graphql';
import { UserModel, UserModelToken } from '../../domain/models/user.model';
import { DomainModule } from '../../domain/domain.module';
import { AddressModelToken, AddressModel } from '../../domain/models/address.model';
import { authChecker } from "../auth/custom-auth-checker";
import { Context } from '../auth/context.interface';
import { Request, request } from 'express';
import { StudentModelToken, StudentModel } from '../../domain/models/student.model';
import { LogInfo } from "../custom-middlewares/middlewares/LogInfo";
import { SharedModule } from '../../shared/shared.module';
// import { StudentModelToken, StudentModel } from '../../domain/models/student.model';
let resolvers = [UserResolver];
export const UserModule: any = new GraphQLModule({
    extraSchemas: [
        buildSchemaSync({
            authChecker,
            resolvers,
            container: ({ ...args }) => {
                return UserModule.injector.getSessionInjector(args.context)
            },
            // container: ({ context }) => context.injector

        })
    ],
    providers: [{
        provide: UserModelToken, useFactory: (injector) => {
            return UserModel;
        },
    }, {
        provide: AddressModelToken, useFactory: (injector) => {
            return AddressModel;
        }
    },
    {
        provide: StudentModelToken, useFactory: (injector) => {
            return StudentModel;
        }
    },
    ...resolvers],
    imports: [DomainModule, SharedModule]
});
