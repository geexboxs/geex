// import { Address } from './address.model';
import { prop, DocumentType, plugin, pre, Ref, mapProp, isDocument } from "@typegoose/typegoose";
import { PhoneNumberResolver, EmailAddressResolver } from "graphql-scalars";
import { ObjectId } from "mongodb";
import { Document, Model, Schema, Types } from "mongoose";
import { ObjectType, Field } from "@nestjs/graphql";
import { NestContainer, ModulesContainer } from "@nestjs/core";
import { Role } from "../../user-manage/model/role.model";
import { UserClaims } from "./user-claim.model";
import { RefType } from "@typegoose/typegoose/lib/types";
import { CommandBus } from "@nestjs/cqrs";
import { UserPermissionChangeCommand } from "../../authorization/commands/change-user-role.command";
import { AccessControl } from "@geexbox/accesscontrol";
import { APP_PERMISSIONS } from "../../authorization/permissions.const";
import { ModelBase, ServiceLocator } from '@geex/api-shared';




@ObjectType()
export class User extends ModelBase<User> {

  @prop()
  @Field()
  public username!: string;
  @prop()
  public passwordHash!: string;
  @prop({
    ref: UserClaims,
    autopopulate: true,
    localField: "_id",
    foreignField: "_id",
  })
  public claims?: Ref<UserClaims>;
  /**
   *
   */
  constructor(username: string, passwordHash: string) {
    super();
    this.username = username;
    this.passwordHash = passwordHash;
  }

  async toContextUser() {
    if (!isDocument(this)) {
      throw Error("entity is not attached to database");
    }

    // let rolePermissions = await Promise.all(this.roles?.map(async x => {
    //     await x._documentContext.populate("role").execPopulate();
    //     return (x.role as Role).permissions;
    // }));
    let roles = ServiceLocator.instance.get(AccessControl).getInheritedRolesOf(this.id);
    return { username: this.username, userId: this.id, ...this.claims, roles } as Express.User;
  }
  async setRoles(roles: string[]) {
    let acc = ServiceLocator.instance.get(AccessControl);
    acc.grant(this.id);
    acc.grant(roles);
    acc.extendRole(this.id, roles, true);
  }
  async setUserPermissions(permissions: string[]) {
    let acc = ServiceLocator.instance.get(AccessControl);
    permissions.forEach(x => {
      acc.grant(this.id).do(x)
    });
    await ServiceLocator.instance.get(CommandBus).execute(new UserPermissionChangeCommand(this));
  }
}
