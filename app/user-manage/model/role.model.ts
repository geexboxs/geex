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
 


 
@ObjectType()
export class Role extends ModelBase {
    @prop({
        ref: Role,
        localField: nameof(User.prototype._id),
        foreignField: nameof(Role.prototype._id),
    })
    public users?: Ref<User>[];

    @prop({
        unique: true,
    })
    @Field()
    public name!: string;
    @prop()
    @Field(_ => [String])
    permissions: string[] = [];

    /**
     *
     */
    constructor(name: string) {
        super();
        this.name = name;
    }
    async setRolePermissions(permissions: string[]) {
        this.permissions = permissions;
        await this._documentContext.save();
    }
}
