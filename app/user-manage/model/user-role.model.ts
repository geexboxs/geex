// import { Address } from './address.model';
import { prop, DocumentType, plugin, pre, Ref } from "@typegoose/typegoose";
import { PhoneNumberResolver, EmailAddressResolver } from "graphql-scalars";
import { ObjectId } from "mongodb";
import { Document, Model, Schema, Types } from "mongoose";
import { ModelBase } from "../../../shared/utils/model-base";
import { Injector } from "@graphql-modules/di";
import { ObjectType, Field } from "@nestjs/graphql";
import { NestContainer, ModulesContainer } from "@nestjs/core";
import { User } from "../../account/models/user.model";
import { Role } from "./role.model";
import { RefType } from "@typegoose/typegoose/lib/types";


@ObjectType()
export class UserRole extends ModelBase {
    @prop({
        ref: Role,
        localField: nameof(UserRole.prototype.roleId),
        foreignField: nameof(Role.prototype.userRoles),
    })
    public Role?: Ref<Role>;
    @prop({
        ref: User,
        localField: nameof(UserRole.prototype.userId),
        foreignField: nameof(User.prototype.userRoles),
    })
    public User?: Ref<User>;
    @prop({
        index: true,
    })
    roleId: RefType;
    @prop({
        index: true,
    })
    userId: RefType;

    /**
     *
     */
    constructor({ userId, roleId }: { userId: RefType, roleId: RefType }) {
        super();
        this.roleId = roleId;
        this.userId = userId;
    }
}
