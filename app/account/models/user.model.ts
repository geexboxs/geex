// import { Address } from './address.model';
import { prop, DocumentType, plugin, pre } from "@typegoose/typegoose";
import { PhoneNumberResolver, EmailAddressResolver } from "graphql-scalars";
import { ObjectId } from "mongodb";
import { Document, Model, Schema, Types } from "mongoose";
import { ModelBase } from "../../../shared/utils/model-base";
import { Injector } from "@graphql-modules/di";
import { ModelFieldResolver } from "../../../types";
import { ObjectType, Field } from "@nestjs/graphql";

@ObjectType()
export class User extends ModelBase {
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
