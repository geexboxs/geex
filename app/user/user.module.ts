import { GraphQLModule } from '@graphql-modules/core';
// import { UserResolver } from './user.resolver';
import { buildSchemaSync, Resolver, Query, ObjectType, Field, Authorized } from 'type-graphql';
import { DomainModule } from '../../domain/domain.module';
import { RoleBasedAuthChecker } from "../../shared/authentication/role-based-auth-checker";
import { GeexRoles } from "../../shared/authentication/roles";
import { UserResolver } from './user.resolver';
// import { StudentModelToken, StudentModel } from '../../domain/models/student.model';
const resolvers = [UserResolver];
export const UserModule: GraphQLModule = new GraphQLModule({
    extraSchemas: [
        buildSchemaSync({
            authChecker: RoleBasedAuthChecker,
            resolvers: [...resolvers],
            container: ({ ...args }) => {
                UserModule.injector.addChild(DomainModule.injector);
                return UserModule.injector.getSessionInjector(args.context);
            },
            // container: ({ context }) => context.injector

        })
    ],
    providers: [...resolvers],
    imports: [DomainModule]
});
