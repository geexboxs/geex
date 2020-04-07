// import { Address } from './address.model';
import { prop, DocumentType, plugin, pre, Ref } from "@typegoose/typegoose";
import { PhoneNumberResolver, EmailAddressResolver } from "graphql-scalars";
import { ObjectId } from "mongodb";
import { Document, Model, Schema, Types } from "mongoose";
import { ModelBase } from "../../../shared/utils/model-base";
import { Injector } from "@graphql-modules/di";
import { ModelFieldResolver, IUserContext } from "../../../types";
import { ObjectType, Field } from "@nestjs/graphql";
import { NestContainer, ModulesContainer } from "@nestjs/core";
import { User } from "../../account/models/user.model";
import { Role } from "./role.model";


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
    roleId: ObjectId;
    @prop({
        index: true,
    })
    userId: ObjectId;

    /**
     *
     */
    constructor({ userId, roleId }: { userId: ObjectId, roleId: ObjectId }) {
        super();
        this.roleId = roleId;
        this.userId = userId;
    }
}
