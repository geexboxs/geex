import { prop, Typegoose } from 'typegoose';
import { Schema } from 'mongoose';
import { Field, ObjectType } from 'type-graphql';
import { User } from './user.model';
@ObjectType()
export class Address extends Typegoose {
    @Field(returns => String)
    _id!: Schema.Types.ObjectId;
    @prop()
    @Field()
    address!: string;
    constructor(init?: Partial<Address>) {
        super();
        Object.assign(this, init);
    }
}

export const AddressModel = new Address().getModelForClass(Address);
export const AddressModelToken = Symbol("AddressModel");

