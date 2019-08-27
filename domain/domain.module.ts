// import { User } from "./models/user.model";

import { GraphQLModule } from "@graphql-modules/core";
import "./utils/typegoose.extension";
import { Typegoose } from "typegoose";
import { User } from "./models/user.model";
// import { Address } from "./models/address.model";

const UserModel = Typegoose.getModel(User);
export const UserModelToken = Symbol("UserModel");
export const DomainModule = new GraphQLModule({
    imports: [],
    providers: [
        {
            provide: UserModelToken, useValue: UserModel
        }
    ]
});
