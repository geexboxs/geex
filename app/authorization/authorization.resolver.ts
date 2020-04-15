import { Resolver, Mutation, Context, Query, CONTEXT, Args, ResolveField, Root, Parent } from "@nestjs/graphql";
import { AccessControl } from "@geexbox/accesscontrol";
import { InjectModel } from "@nestjs/mongoose";
import { User } from "../account/models/user.model";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { Inject, ExecutionContext, UseGuards } from "@nestjs/common";
import { Permission } from "./models/permission.model";
import { REQUEST } from "@nestjs/core";
import { AuthGuard } from "./utils/jwt.guard";


@Resolver((of) => Permission)
export class AuthorizationResolver {
    constructor(
        @InjectModel(nameof(User))
        private userModel: ModelType<User>,
        @InjectModel(nameof(Permission))
        private permissionModel: ModelType<Permission>,
        @Inject(AccessControl)
        private ac: AccessControl,
        @Inject(CONTEXT)
        private context: ExecutionContext,
    ) { }
    @UseGuards(AuthGuard)
    @Query(() => [Permission])
    public async getAllPermissions() {
        return (await this.permissionModel.find().exec());
    }
    @UseGuards(AuthGuard)
    @Mutation(() => [Permission])
    public async createPermissions(@Args({ name: "permissions", type: () => [String] }) permissions: [string]) {
        return await Promise.all(permissions.map(async x => {
            try {
                return await this.permissionModel.create(new Permission({ name: x }));
            } catch (error) {
                return new Permission({ name: x });
            }
        }));
    }

    @UseGuards(AuthGuard)
    @Query(() => [Permission])
    public async getMyPermissions() {
        return (await this.userModel.findById(this.context.req.user?.userId).exec())?.permissions;
    }

    @UseGuards(AuthGuard)
    @ResolveField(() => [Permission])
    async childPermissions(@Parent() parent: Permission): Promise<Permission[]> {
        return await parent.childPermissions();
    }
}
