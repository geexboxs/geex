// import { Address } from './address.model';
import { prop } from "@typegoose/typegoose";
import { PhoneNumberResolver, EmailAddressResolver } from "graphql-scalars";
import { ObjectId } from "mongodb";
import { Document, Model, Schema, Types } from "mongoose";
import { Authorized, Field, ObjectType, UseMiddleware } from "type-graphql";
import { ModelBase } from "../../../shared/utils/model-base";
import { PasswordHasher } from "../utils/password-hasher";

@ObjectType()
export class UserProfile {
    @prop()
    @Field()
    public firstName: string = "";
    @prop()
    @Field()
    public lastName: string = "";
    @prop()
    @Field()
    public avatarUrl: string = "";
}

@ObjectType()
export class User extends ModelBase {

    static create(username: string, password: string, hasher: PasswordHasher) {
        const newUser = new User();
        newUser.username = username;
        newUser.passwordHash = hasher.hash(password);
        return newUser;
    }

    @prop()
    @Field()
    public username!: string;
    @prop()
    public passwordHash!: string;
    @prop()
    @Field((type) => [String])
    public roles!: string[];
    @prop()
    @Field((type) => UserProfile)
    public profile: UserProfile = new UserProfile();
    @prop()
    @Field((type) => PhoneNumberResolver)
    public phone?: string;
    @prop()
    @Field((type) => EmailAddressResolver)
    public email?: string;
}
