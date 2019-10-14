import { pre, prop, Typegoose } from "@typegoose/typegoose";
import { Schema } from "mongoose";
import { Field } from "type-graphql";

@pre<ModelBase>("save", function(next) {
    if (!this) {
        next();
    }
    if (this.createAt === undefined) {
        this.createAt = new Date();
    }
    this.updateAt = new Date();
    next();
})
export abstract class ModelBase {
    @Field((returns) => String)
    public _id!: Schema.Types.ObjectId;

    @prop()
    @Field()
    public createAt!: Date;
    @prop()
    @Field()
    public updateAt!: Date;
}
