// import { Address } from './address.model';
import { prop, instanceMethod, DocumentType, plugin, pre } from "@typegoose/typegoose";
import { PhoneNumberResolver, EmailAddressResolver } from "graphql-scalars";
import { ObjectId } from "mongodb";
import { Document, Model, Schema, Types } from "mongoose";
import { Authorized, Field, ObjectType, UseMiddleware } from "type-graphql";
import { ModelBase } from "../../../shared/utils/model-base";
import { Injector } from "@graphql-modules/di";
import { ModelFieldResolver } from "../../../types";

@ObjectType()
export class User extends ModelBase {
    @Field()
    public get roles(): Promise<string[]> | undefined {
        return User._getterImplementions["roles"] && User._getterImplementions["roles"]();
    }
    @Field()
    public get permissions(): string[] | undefined {
        return User._getterImplementions["permissions"] && User._getterImplementions["permissions"]();
    }
    public static setDependentImplementionForModel<T extends keyof User>(key: T, implemention: ModelFieldResolver<User, T>) {
        this._getterImplementions.set(key, implemention);
    }
    private static _getterImplementions: Map<keyof User, ModelFieldResolver<User>> = new Map();
    @prop()
    @Field()
    public username!: string;
    @prop()
    public passwordHash!: string;
    @prop()
    @Field()
    public avatarUrl: string = "";
    @prop()
    @Field((type) => PhoneNumberResolver)
    public phone?: string;
    @prop()
    @Field((type) => EmailAddressResolver)
    public email?: string;

    /**
     *
     */
    constructor(username: string, passwordHash: string) {
        super();
        this.username = username;
        this.passwordHash = passwordHash;
    }
}
