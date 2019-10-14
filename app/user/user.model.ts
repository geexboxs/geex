// import { Address } from './address.model';
import { prop } from "@typegoose/typegoose";
import { PhoneNumberResolver } from "graphql-scalars";
import { ObjectId } from "mongodb";
import { Document, Model, Schema, Types } from "mongoose";
import { Authorized, Field, ObjectType, UseMiddleware } from "type-graphql";
import { ModelBase } from "../../shared/utils/model-base";

@ObjectType()
export class UserProfile {
    @prop()
    @Field()
    public firstName: string = "";
    @prop()
    @Field()
    public lastName: string = "";
}

@ObjectType()
export class User extends ModelBase {

    @prop()
    @Field()
    public username!: string;
    @prop()
    @Field((type) => [String])
    public roles!: string[];
    @prop()
    @Field((type) => UserProfile)
    public profile!: UserProfile;
    @prop()
    @Field((type) => PhoneNumberResolver)
    public phone!: string;
    /**
     *
     */
    constructor() {
        super();
    }
}
