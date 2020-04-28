import { Resolver, Mutation, ID, Args, Query } from "@nestjs/graphql";
import { User } from "../account/models/user.model";
import { InjectModel } from "@nestjs/mongoose";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { Optional, Inject } from "@nestjs/common";
import { EmailSender } from "../../shared/utils/email-sender";
import { REQUEST } from "@nestjs/core";
import { ExecutionContext } from "graphql/execution/execute";
import { RegisterInput } from "../../../shared/contracts/inputs/register.input";
import { VerifyType } from "../account/models/verify-type";
import { I18N } from "../../shared/utils/i18n";
import { Role } from "./model/role.model";
import { ObjectID } from "mongodb";
import { SessionStore } from "../authentication/models/session.model";
import { AccessControl } from "@geexbox/accesscontrol";

@Resolver((of) => User)
export class UserManageResolver {
    constructor(
        @InjectModel(nameof(User))
        private userModel: ModelType<User>,
        // @Optional()
        // @Inject(EmailSender)
        // private emailSender: EmailSender,
        @Inject(AccessControl)
        private ac: AccessControl,
        @Inject(SessionStore)
        private sessionStore: SessionStore,
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
        // await this.sessionStore.createOrRefresh(await user.toExpressUser());
        return true;
    }

    @Mutation(() => Boolean)
    public async assignUserPermission(@Args("identifier") identifier: string, @Args({ name: "permissions", type: () => [String] }) permissions: string[]) {
        let user;
        if (ObjectID.isValid(identifier)) {
            user = await this.userModel.findOne({ _id: identifier });
        } else {
            user = await this.userModel.findOne({ username: identifier });
        }
        if (user == null) {
            throw Error("user not found");
        }
        await user.setUserPermissions(permissions);
        return true;
    }
    @Mutation(() => Boolean)
    public async assignRolePermission(@Args({ name: "roles", type: () => [String] }) roles: [string], @Args({ name: "permissions", type: () => [String] }) permissions: string[]) {
        if (roles?.any() && permissions?.any()) {
            roles.forEach(role => {
                permissions.forEach(permission => {
                    this.ac.grant(role).do(permission);
                });
            });
            return true;
        }

        throw Error("role not found");
    }
}
