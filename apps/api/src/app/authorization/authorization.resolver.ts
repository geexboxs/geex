import { Resolver, Mutation, Context, Query, CONTEXT, Args, ResolveField, Root, Parent } from "@nestjs/graphql";
import { AccessControl } from "@geexbox/accesscontrol";
import { InjectModel } from "@nestjs/mongoose";
import { User } from "../account/models/user.model";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { Inject, ExecutionContext, UseGuards } from "@nestjs/common";
import { REQUEST } from "@nestjs/core";
import { AuthGuard } from "./utils/auth.guard";
import { PermissionScalar } from "./scalars/permission.scalar";
import { AppPermission, APP_PERMISSIONS } from "./permissions.const";
import { PermissionNode } from "./models/permission.model";
import { Role } from "../user-manage/model/role.model";
import { Uow } from "@geex/api-shared";


@Resolver((of) => PermissionNode)
export class AuthorizationResolver {
  constructor(
    @InjectModel(User.name)
    private userModel: ModelType<User>,
    @Inject(AccessControl)
    private ac: AccessControl,
    @Inject(CONTEXT)
    private context: ExecutionContext,
  ) { }
  @UseGuards(AuthGuard("permission:query:any"))
  @Query(() => [PermissionScalar])
  public getAllPermissions() {
    return Object.values(APP_PERMISSIONS);
  }

  @UseGuards(AuthGuard())
  @Query(() => [PermissionScalar])
  public async getMyPermissions() {
    let user = await this.userModel.findById(this.context.req.user?.userId).exec();
    return this.ac.getPermissionsOf(user?.id, true);
  }

  @UseGuards(AuthGuard())
  @ResolveField(() => [PermissionNode])
  async childPermissions(@Parent() parent: PermissionNode) {
    return parent.childPermissions;
  }

  @Mutation(() => Boolean)
  @Uow()
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
