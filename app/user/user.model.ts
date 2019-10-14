// import { Address } from './address.model';
import { Model, Types, Document, Schema } from 'mongoose';
import { ObjectType, Field, Authorized, UseMiddleware } from 'type-graphql';
import { ObjectId } from 'mongodb';
import { ModelBase } from '../../shared/utils/model-base';
import { PhoneNumberResolver } from 'graphql-scalars';
import { prop } from '@typegoose/typegoose';

@ObjectType()
export class UserProfile {
    @prop()
    @Field()
    firstName: string = "";
    @prop()
    @Field()
    lastName: string = "";
}

@ObjectType()
export class User extends ModelBase {
    /**
     *
     */
    constructor() {
        super();
    }

    @prop()
    @Field()
    username!: string;
    @prop()
    @Field(type => [String])
    roles!: string[];
    @prop()
    @Field(type => UserProfile)
    profile!: UserProfile;
    @prop()
    @Field(type => PhoneNumberResolver)
    phone!: string;
}
