import { User, UserModel } from "./models/user.model";

import { AddressModel } from "./models/address.model";
import { GraphQLModule } from "@graphql-modules/core";
import { UserModule } from "../app/user/user.module";
import { SharedModule } from "../shared/shared.module";

User.repo = UserModel;
User.addressRepo = AddressModel;

export const DomainModule = new GraphQLModule({
    imports: [SharedModule]
});
