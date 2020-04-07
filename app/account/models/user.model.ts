// import { Address } from './address.model';
import { prop, DocumentType, plugin, pre, Ref, mapProp, isDocument } from "@typegoose/typegoose";
import { PhoneNumberResolver, EmailAddressResolver } from "graphql-scalars";
import { ObjectId } from "mongodb";
import { Document, Model, Schema, Types } from "mongoose";
import { ModelBase } from "../../../shared/utils/model-base";
import { Injector } from "@graphql-modules/di";
import { ModelFieldResolver, IUserContext } from "../../../types";
import { ObjectType, Field } from "@nestjs/graphql";
import { NestContainer, ModulesContainer } from "@nestjs/core";
import { Role } from "../../user-manage/model/role.model";
import { UserClaims } from "./user-claim.model";
import { UserRole } from "../../user-manage/model/user-role.model";


@ObjectType()
export class User extends ModelBase {

    @prop()
    @Field()
    public username!: string;
    @prop()
    @Field()
    public test!: number;
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
    /**
     *
     */
    constructor(username: string, passwordHash: string) {
        super();
        this.username = username;
        this.passwordHash = passwordHash;
        this.test = 5;
    }

    toUserContext(this: DocumentType<User>) {
        if (!isDocument(this)) {
            throw Error("entity is not attached to database");
        }
        return { username: this.username, id: this.id, ...this.claims } as IUserContext;
    }
    async setRoles(this: DocumentType<User>, roles: string[]) {
        await this.model(UserRole).deleteMany({ userId: this._id }).exec();
        let roleEntities = await Promise.all(roles.map(async x => await this.model(Role).findOneAndUpdate({ name: x }, { name: x }, { upsert: true, new: true }).exec()));
        this.userRoles = (await this.model(UserRole).create(roleEntities.map(x => new UserRole({ userId: this._id, roleId: x._id })))) as DocumentType<UserRole>[];
    }
}
