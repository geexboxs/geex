// import { User } from "./models/user.model";

import { GraphQLModule } from "@graphql-modules/core";
import { UserModule } from "../app/user/user.module";
import { Typegoose } from "typegoose";
import "./utils/typegoose.extension";
// import { Address } from "./models/address.model";

// const UserModel = Typegoose.getModel(User);
export const UserModelToken = Symbol("UserModel");

// const AddressModel = Typegoose.getModel(Address);
export const AddressModelToken = Symbol("AddressModel");
export const DomainModule = new GraphQLModule({
    imports: [],
    // providers: [
    //     {
    //         provide: UserModelToken, useValue: UserModel
    //     },{
    //         provide: AddressModelToken, useValue: AddressModel
    //     }
    // ]
});
