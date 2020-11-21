// import { Address } from './address.model';
import { prop, DocumentType, plugin, pre, Ref, mapProp, isDocument } from "@typegoose/typegoose";
import { PhoneNumberResolver, EmailAddressResolver } from "graphql-scalars";
import { ObjectId } from "mongodb";
import { Document, Model, Schema, Types } from "mongoose";
import { ObjectType, Field } from "@nestjs/graphql";
import { NestContainer, ModulesContainer } from "@nestjs/core";
import { Role } from "../../user-manage/model/role.model";
import { RefType } from "@typegoose/typegoose/lib/types";
import { CommandBus } from "@nestjs/cqrs";
import { UserPermissionChangeCommand } from "../../authorization/commands/change-user-role.command";
import { AccessControl } from "@geexbox/accesscontrol";
import { APP_PERMISSIONS } from "../../authorization/permissions.const";
import { ModelBase, ServiceLocator } from '@geex/api-shared';
import { User } from '../../account/models/user.model';

@ObjectType()
export class Notification extends ModelBase<Notification> {
  public init(...args: any) {
    throw new Error('Method not implemented.');
  }
  @prop()
  public data!: string;
  @prop()
  public category!: string;
  @prop()
  public severity!: string;
  @prop({
    ref: User,
    autopopulate: true,
    localField: "_id",
    foreignField: "_id",
  })
  public senderId?: Ref<User>;

  @prop({
    ref: User,
    autopopulate: true,
    localField: "_id",
    foreignField: "_id",
  })
  public receiverIds?: Ref<User>;
}
