import { GraphQLModule } from "@graphql-modules/core";
import { Authorized, buildSchemaSync, Field, ObjectType, Query, Resolver } from "type-graphql";
import { GeexRoles } from "../../shared/auth/roles";
import { GeexContext } from "../../shared/utils/abstractions";
import { UserResolver } from "./user.resolver";
const resolvers = [UserResolver];
export const UserModule: GraphQLModule = new GraphQLModule({
    extraSchemas: [
        buildSchemaSync({
            resolvers: [UserResolver],
            container: ({ ...args }) => {
                return {
                    get(someClass, resolverData) {
                        return (resolverData.context as GeexContext).injector.get(someClass);
                    },
                };
            },
        }),
    ],
    providers: [...resolvers],
    imports: [],
});
