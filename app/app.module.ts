import { GraphQLModule } from "@graphql-modules/core";
import { buildSchemaSync } from "type-graphql";
import { UserModule } from "./user/user.module";
import { SharedModule } from "../shared/shared.module";

export const AppModule = new GraphQLModule({
    imports: [SharedModule, UserModule],
});
