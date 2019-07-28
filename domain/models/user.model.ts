import { prop, Typegoose, pre, arrayProp, Ref, instanceMethod, ModelType, InstanceType, plugin } from 'typegoose';
import { Address } from './address.model';
import { Model, Types, Document, Schema } from 'mongoose';
import { ObjectType, Field, Authorized, UseMiddleware } from 'type-graphql';
import { CreateUserInput } from '../../utils/CreateUserInput';
import { isObject } from 'typegoose/lib/utils';
import { ObjectId } from 'mongodb';
import { ModelBase } from '../utils/model-base';
import { PhoneNumber } from '@okgrow/graphql-scalars';


@ObjectType()
export class User extends ModelBase {
    async init(input: CreateUserInput) {
        this.name = input.name;
    }
    /**
     *
     */
    constructor() {
        super();
    }

    @prop()
    @Field()
    name!: string;
    @prop()
    password!: string;
    @arrayProp({ itemsRef: Address })
    protected _addresses!: Ref<Address>[];
    @Field(type => [Address])
    public get addresses(this: InstanceType<User>): Address[] {
        if (this._addresses[0] instanceof ObjectId) {
            this._addresses = this.populate("_address")._addresses;
        }
        return this._addresses as Address[];
    }
    @prop()
    @Field(type => [String])
    roles!: string[];
    @prop()
    @Field(type => String)
    profile!: UserProfile;
    @prop()
    @Field(type => PhoneNumber)
    phone!: string;
}
interface UserProfile {

}
