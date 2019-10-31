import { pre, prop, Typegoose } from "@typegoose/typegoose";
import { Field } from "type-graphql";
import { ObjectId } from "mongodb";
import { ModelFieldResolver } from "../../types";

@pre<ModelBase>("save", function (next) {
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
    // tslint:disable-next-line: variable-name
    public _id!: ObjectId;

    @Field((returns) => String)
    public get id() {
        return this._id.toHexString();
    }

    @prop()
    @Field()
    public createAt!: Date;
    @prop()
    @Field()
    public updateAt!: Date;
}
