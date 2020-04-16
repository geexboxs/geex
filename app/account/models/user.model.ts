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
import { RefType } from "@typegoose/typegoose/lib/types";




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
        localField: nameof(User.prototype._id),
        foreignField: nameof(UserClaims.prototype._id),
    })
    public claims?: Ref<UserClaims>;
    @prop({
        ref: Role,
        autopopulate: true,
        localField: nameof(User.prototype.roleIds),
        foreignField: nameof(Role.prototype._id),
    })
    public readonly roles?: Array<Role>;
    @prop()
    public roleIds?: RefType[];
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

    async toExpressUser() {
        if (!isDocument(this)) {
            throw Error("entity is not attached to database");
        }

        // let rolePermissions = await Promise.all(this.roles?.map(async x => {
        //     await x._documentContext.populate("role").execPopulate();
        //     return (x.role as Role).permissions;
        // }));
        let rolePermissions = this.roles?.mapMany(x => x.permissions) ?? [];
        return { username: this.username, userId: this.id, ...this.claims, roles: this.roles?.map(x => x.name), scopes: this.permissions.concat(rolePermissions) } as Express.User;
    }
    async setRoles(roles: string[]) {
        let roleEntities = await Promise.all(roles.map(async x => await this._documentContext.model(Role).findOneAndUpdate({ name: x }, { name: x }, { upsert: true, new: true }).exec()));
        this.roleIds = roleEntities.map(x => x._id);
        await this._documentContext.save();
        // await this._documentContext.update({ $set: { roleIds: roleEntities.map(x => x._id) } }).exec();
    }
    async setUserPermissions(permissions: string[]) {
        this.permissions = permissions;
        await this._documentContext.save();
    }
}
