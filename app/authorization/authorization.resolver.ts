import { Resolver, Mutation } from "@nestjs/graphql";
import { AccessControl } from "@geexbox/accesscontrol";
import { InjectModel } from "@nestjs/mongoose";
import { User } from "../account/models/user.model";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { Inject } from "@nestjs/common";
import { IGeexContext } from "../../types";
import { Permission } from "./models/permission.model";


@Resolver((of) => Permission)
export class AuthenticationResolver {
    constructor(
        @InjectModel(nameof(User))
        private userModel: ModelType<User>,
        @Inject(AccessControl)
        private ac: AccessControl,
    ) { }
    @Mutation(() => Permission)
    public async getPermissions() {
        return true;
    }

}
