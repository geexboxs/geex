import { GraphQLModule } from '@graphql-modules/core';
import { buildSchemaSync, Resolver, Query, ObjectType, Field, Authorized } from 'type-graphql';
import { GeexRoles } from "../../shared/authentication/roles";
import { DefaultAuthChecker } from '../../shared/authentication/auth-checker';
import { UserResolver } from './user.resolver';
import { GeexContext } from '../../shared/utils/abstractions';
const resolvers = [UserResolver];
export const UserModule: GraphQLModule = new GraphQLModule({
    extraSchemas: [
        buildSchemaSync({
            authChecker: DefaultAuthChecker,
            resolvers: [UserResolver],
            container: ({ ...args }) => {
                return {
                    get(someClass, resolverData) {
                        return (resolverData.context as GeexContext).injector.get(someClass);
                    }
                }
            },
        })
    ],
    providers: [...resolvers],
    imports: []
});
