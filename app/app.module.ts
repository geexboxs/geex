import { GraphQLModule } from "@graphql-modules/core";
import { buildSchemaSync } from "type-graphql";
import { UserModule } from "./user/user.module";

export const AppModule = new GraphQLModule({
    imports: [UserModule],
});
