import { prop, Typegoose } from 'typegoose';
import { Schema } from 'mongoose';
import { Field, ObjectType } from 'type-graphql';
import { User } from './user.model';
import { ModelBase } from '../utils/model-base';
@ObjectType()
export class Address extends ModelBase {
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
