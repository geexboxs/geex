import { ObjectId } from "bson";
import * as mongoose from "mongoose";
import * as mongodb from "mongodb";
import { Document } from "mongoose";
import { staticImplements } from "./decorators";
import { Typegoose, GetModelForClassOptions } from "typegoose";

Typegoose.getModel = function (ctor: any, options?: GetModelForClassOptions): any {
    return new ctor().getModelForClass(ctor, options);
}
/** fields not in base class of mongoose Document. */
export type GeexEntityIntersection<T = any> = Partial<Omit<T, keyof Document>>;
export type GeexPrimitive = string | number | bigint | boolean | symbol | String | Number | Date | Boolean | BigInt | Symbol | ObjectId | undefined;

export type PrimitiveIntersection<T> = {
    [key in keyof T]: T[key] extends GeexPrimitive ? key : never
}[keyof T];

/** concrete fields that can be used in doc query. */
export type QueryableIntersection<T> = Omit<{
    [key in PrimitiveIntersection<T>]?: T[key]
}, keyof Omit<Document, keyof { id }>>

// @staticImplements<TypegooseStatic>()   /* this statement implements both normal interface & static interface */
// export class GeexTypegoose extends Typegoose { /* implements MyType { */ /* so this become optional not required */
//     public static getModel(options?: GetModelForClassOptions) {
//         return this.prototype.getModelForClass(this);
//     }
//     // instanceMethod() { }
// }
