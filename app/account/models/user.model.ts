// import { Address } from './address.model';
import { prop, DocumentType, plugin, pre, Ref, mapProp, isDocument } from "@typegoose/typegoose";
import { PhoneNumberResolver, EmailAddressResolver } from "graphql-scalars";
import { ObjectId } from "mongodb";
import { Document, Model, Schema, Types } from "mongoose";
import { ModelBase } from "../../../shared/utils/model-base";
import { Injector } from "@graphql-modules/di";
import { ObjectType, Field } from "@nestjs/graphql";
import { NestContainer, ModulesContainer } from "@nestjs/core";
import { Role } from "../../user-manage/model/role.model";
import { UserClaims } from "./user-claim.model";
import { UserRole } from "../../user-manage/model/user-role.model";


@ObjectType()
export class User extends ModelBase<User> {

    @prop()
    @Field()
    public username!: string;
    @prop()
    public passwordHash!: string;
    @prop({
        ref: UserClaims,
        localField: nameof(User.prototype._id),
        foreignField: nameof(UserClaims.prototype.userId),
    })
    public claims?: Ref<UserClaims>;
    @prop({
        ref: Role,
        localField: nameof(Role.prototype._id),
        foreignField: nameof(UserRole.prototype.userId),
    })
    public userRoles?: Array<Ref<UserRole>>;
    @prop()
    @Field(_ => [String])
    public permissions: string[] = [];
    /**
     *
     */
    constructor(username: string, passwordHash: string) {
        super();
        this.username = username;
        this.passwordHash = passwordHash;
    }

    toUserContext() {
        if (!isDocument(this)) {
            throw Error("entity is not attached to database");
        }
        return { username: this.username, userId: this.id, ...this.claims } as Express.User;
    }
    async setRoles(roles: string[]) {
        await this._documentContext.model(UserRole).deleteMany({ userId: this._id }).exec();
        let roleEntities = await Promise.all(roles.map(async x => await this._documentContext.model(Role).findOneAndUpdate({ name: x }, { name: x }, { upsert: true, new: true }).exec()));
        this.userRoles = (await this._documentContext.model(UserRole).create(roleEntities.map(x => new UserRole({ userId: this._id, roleId: x._id })))) as DocumentType<UserRole>[];
    }
    async setUserPermissions(permissions: string[]) {
        this.permissions = permissions;
        await this._documentContext.save();
    }
}
