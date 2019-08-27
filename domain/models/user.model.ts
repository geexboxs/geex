import { prop, Typegoose, pre, arrayProp, Ref, instanceMethod, ModelType, InstanceType, plugin } from 'typegoose';
// import { Address } from './address.model';
import { Model, Types, Document, Schema } from 'mongoose';
import { ObjectType, Field, Authorized, UseMiddleware } from 'type-graphql';
import { isObject } from 'typegoose/lib/utils';
import { ObjectId } from 'mongodb';
import { ModelBase } from '../utils/model-base';
import { PhoneNumber } from '@okgrow/graphql-scalars';
import { User as AccountUser } from '@accounts/types';

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
    @Field(type => PhoneNumber)
    phone!: string;
}
