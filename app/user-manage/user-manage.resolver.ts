import { Resolver, Mutation, ID, Args, Query } from "@nestjs/graphql";
import { User } from "../account/models/user.model";
import { InjectModel } from "@nestjs/mongoose";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { Optional, Inject } from "@nestjs/common";
import { EmailSender } from "../../shared/utils/email-sender";
import { REQUEST } from "@nestjs/core";
import { ExecutionContext } from "graphql/execution/execute";
import { RegisterInput } from "../account/models/register.input";
import { VerifyType } from "../account/models/verify-type";
import { I18N } from "../../shared/utils/i18n";
import { Role } from "./model/role.model";


@Resolver((of) => User)
export class UserManageResolver {
    constructor(
        @InjectModel(nameof(User))
        private userModel: ModelType<User>,
        @InjectModel(nameof(Role))
        private roleModel: ModelType<Role>,
        @Optional()
        @Inject(EmailSender)
        private emailSender: EmailSender,
        @Inject(REQUEST)
        private request: ExecutionContext,
    ) { }

    @Mutation(() => Boolean)
    public async assignRole(@Args("identifier") identifier: string, @Args({ name: "roles", type: () => [String] }) roles: string[]) {
        let user = await this.userModel.findOne({ $or: [{ username: identifier }, { _id: identifier }] });
        if (user == null) {
            throw Error("user not found");
        }
        await user.setRoles(roles);
        return true;
    }

    @Mutation(() => Boolean)
    public async assignUserPermission(@Args("identifier") identifier: string, @Args({ name: "permissions", type: () => [String] }) permissions: string[]) {
        let user = await this.userModel.findOne({ $or: [{ username: identifier }, { _id: identifier }] });
        if (user == null) {
            throw Error("user not found");
        }
        await user.setUserPermissions(permissions);
        return true;
    }
    @Mutation(() => Boolean)
    public async assignRolePermission(@Args({ name: "roles", type: () => [String] }) roleNames: [string], @Args({ name: "permissions", type: () => [String] }) permissions: string[]) {
        let roles = await this.roleModel.find({ name: { $in: roleNames } }).exec();
        if (roles?.length > 1) {
            await Promise.all(roles.map(x => x.setRolePermissions(permissions)));
            return true;
        }
        throw Error("user not found");
    }
}
