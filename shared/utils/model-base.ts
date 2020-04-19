import { pre, prop, Typegoose, DocumentType } from "@typegoose/typegoose";
import { Field } from "@nestjs/graphql";
import { ObjectId } from "mongodb";
import { RefType } from "@typegoose/typegoose/lib/types";

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
export abstract class ModelBase<T = any> {
    // tslint:disable-next-line: variable-name
    public _id!: ObjectId;

    @Field((returns) => String)
    public get id() {
        return this._id.toString();
    }

    @prop()
    @Field()
    public createAt!: Date;
    @prop()
    @Field()
    public updateAt!: Date;

    public get _documentContext() {
        return this as unknown as DocumentType<T>;
    }
}
