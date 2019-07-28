import { Field } from "type-graphql";
import { prop, pre, Typegoose } from "typegoose";
import { Schema } from "mongoose";

@pre<ModelBase>('save', function (next) {
    if (!this) {
        next();
    }
    if (this.createAt === undefined) {
        this.createAt = new Date();
    }
    this.updateAt = new Date();
    next();
})
export abstract class ModelBase extends Typegoose {
    @Field(returns => String)
    _id!: Schema.Types.ObjectId;

    @prop()
    @Field()
    createAt!: Date;
    @prop()
    @Field()
    updateAt!: Date;
    init<T>(...args: any[]) { };
}
