import { GraphQLModule } from "@graphql-modules/core";
import { Authorized, buildSchemaSync, Field, ObjectType, Query, Resolver } from "type-graphql";
import { GeexRoles } from "../../shared/auth/roles";
import { IGeexContext } from "../../types";
import { UserResolver } from "./user.resolver";
import { getModelForClass } from "@typegoose/typegoose";
import { User } from "./user.model";
import { UserModelToken } from "../../shared/tokens";
const resolvers = [UserResolver];
export const UserModule: GraphQLModule = new GraphQLModule({
    extraSchemas: [
        buildSchemaSync({
            container: ({ ...args }) => {
                return {
                    get(someClass, resolverData) {
                        return (resolverData.context as IGeexContext).injector.get(someClass);
                    },
                };
            },
            resolvers: [UserResolver],
        }),
    ],
    imports: [],
    providers: [{
        provide: UserModelToken,
        useFactory: (provider) => getModelForClass(User),
    }, ...resolvers],
});
