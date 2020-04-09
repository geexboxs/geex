import { ObjectId } from "bson";
import mongodb = require('mongodb');
import { Document } from "mongoose";
import { User } from "../app/account/models/user.model";
import { UserRole } from "../app/user-manage/model/user-role.model";
import { ModelType, RefType } from "@typegoose/typegoose/lib/types";
import { MongooseDocument } from "mongoose";
import { Connection } from "mongoose";
import { Schema } from "mongoose";
import { ResolverTypeFn } from "@nestjs/graphql";
import { Type } from "@nestjs/common";
import { Model } from "mongoose";

/** fields not in base class of mongoose Document. */
export type GeexEntityIntersection<T = any> = Partial<Omit<T, keyof Document>>;
export type GeexPrimitive = string | number | bigint | boolean | symbol | String | Number | Date | Boolean | BigInt | Symbol | ObjectId | undefined;

export type PrimitiveKeys<T> = {
    [key in keyof T]: T[key] extends GeexPrimitive ? key : never
}[keyof T];

export type PrimitiveProps<T> = { [key in PrimitiveKeys<T>]?: T[key] };

export type QuerySelector<T> = {
    [key in keyof T]: {
        $gt?: T[key],
        $gte?: T[key],
        $lt?: T[key],
        $lte?: T[key],
        $ne?: T[key],
        $in?: T[key][],
        $nin?: T[key][],

    }
} | {
        [key in keyof T]: {
            $regex?: RegExp,
            $options?: any
        }
    };
type FieldKeys<T> = Exclude<keyof PrimitiveProps<T>, keyof Document> | PrimitiveKeys<{ _id?: ObjectId, id?: string, __v?: number }>;
type FieldProps<T> = Omit<PrimitiveProps<T>, keyof Document> | PrimitiveProps<{ _id?: ObjectId, id?: string, __v?: number }>;
type OperationProps<T> = QuerySelector<FieldProps<T>>;
type LogicOperationProps<T> = {
    $and?: (FieldProps<T> | OperationProps<T>)[],
    $nor?: (FieldProps<T> | OperationProps<T>)[],
    $or?: (FieldProps<T> | OperationProps<T>)[],
};

export type ConditionObject<T> =
    // 基本字段查询
    FieldProps<T> |
    // 四则运算查询
    OperationProps<T> |
    // 逻辑运算查询
    LogicOperationProps<T>;

declare module "mongoose" {
    export interface Model<T extends Document, QueryHelpers = {}> extends NodeJS.EventEmitter, ModelProperties {
        /**
         * Finds a single document by its _id field. findById(id) is almost*
         * equivalent to findOne({ _id: id }). findById() triggers findOne hooks.
         * @param id value of _id to query by
         * @param projection optional fields to return
         */
        findById(id: RefType,
            callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;
        findById(id: RefType, projection: any,
            callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;
        findById(id: RefType, projection: any, options: any,
            callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;

        /** Counts number of matching documents in a database collection. */
        count(conditions: ConditionObject<T>, callback?: (err: any, count: number) => void): Query<number> & QueryHelpers;

        /**
         * Shortcut for saving one or more documents to the database. MyModel.create(docs)
         * does new MyModel(doc).save() for every doc in docs.
         * Triggers the save() hook.
         */
        create(docs: any[], callback?: (err: any, res: T[]) => void): Promise<T[]>;
        create(docs: any[], options?: SaveOptions, callback?: (err: any, res: T[]) => void): Promise<T[]>;
        create(...docs: any[]): Promise<T>;
        create(...docsWithCallback: any[]): Promise<T>;

        /** Creates a Query for a distinct operation. Passing a callback immediately executes the query. */
        distinct(field: string, callback?: (err: any, res: any[]) => void): Query<any[]> & QueryHelpers;
        distinct(field: string, conditions: ConditionObject<T>,
            callback?: (err: any, res: any[]) => void): Query<any[]> & QueryHelpers;

        /**
         * Returns true if at least one document exists in the database that matches
         * the given `filter`, and false otherwise.
         */
        exists(filter: any, callback?: (err: any, res: boolean) => void): Promise<boolean>;

        /**
         * Finds documents.
         * @param projection optional fields to return
         */
        find(callback?: (err: any, res: T[]) => void): DocumentQuery<T[], T> & QueryHelpers;
        find(conditions: ConditionObject<T>, callback?: (err: any, res: T[]) => void): DocumentQuery<T[], T> & QueryHelpers;
        find(conditions: ConditionObject<T>, projection?: any | null,
            callback?: (err: any, res: T[]) => void): DocumentQuery<T[], T> & QueryHelpers;
        find(conditions: ConditionObject<T>, projection?: any | null, options?: any | null,
            callback?: (err: any, res: T[]) => void): DocumentQuery<T[], T> & QueryHelpers;



        /**
         * Issue a mongodb findAndModify remove command by a document's _id field.
         * findByIdAndRemove(id, ...) is equivalent to findOneAndRemove({ _id: id }, ...).
         * Finds a matching document, removes it, passing the found document (if any) to the callback.
         * Executes immediately if callback is passed, else a Query object is returned.
         *
         * If mongoose option 'useFindAndModify': set to false it uses native findOneAndUpdate() rather than deprecated findAndModify().
         * https://mongoosejs.com/docs/api.html#mongoose_Mongoose-set
         *
         * Note: same signatures as findByIdAndDelete
         *
         * @param id value of _id to query by
         */
        findByIdAndRemove(): DocumentQuery<T | null, T> & QueryHelpers;
        findByIdAndRemove(id: any | number | string,
            callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;
        findByIdAndRemove(id: any | number | string, options: QueryFindOneAndRemoveOptions,
            callback?: (err: any, res: mongodb.FindAndModifyWriteOpResultObject<T | null>) => void)
            : Query<mongodb.FindAndModifyWriteOpResultObject<T | null>> & QueryHelpers;
        findByIdAndRemove(id: any | number | string, options: QueryFindOneAndRemoveOptions, callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;


        /**
        * Issue a mongodb findOneAndDelete command by a document's _id field.
        * findByIdAndDelete(id, ...) is equivalent to findByIdAndDelete({ _id: id }, ...).
        * Finds a matching document, removes it, passing the found document (if any) to the callback.
        * Executes immediately if callback is passed, else a Query object is returned.
        *
        * Note: same signatures as findByIdAndRemove
        *
        * @param id value of _id to query by
        */
        findByIdAndDelete(): DocumentQuery<T | null, T> & QueryHelpers;
        findByIdAndDelete(id: any | number | string,
            callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;
        findByIdAndDelete(id: any | number | string, options: QueryFindOneAndRemoveOptions,
            callback?: (err: any, res: mongodb.FindAndModifyWriteOpResultObject<T | null>) => void)
            : Query<mongodb.FindAndModifyWriteOpResultObject<T | null>> & QueryHelpers;
        findByIdAndDelete(id: any | number | string, options: QueryFindOneAndRemoveOptions, callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;

        /**
         * Issues a mongodb findAndModify update command by a document's _id field. findByIdAndUpdate(id, ...)
         * is equivalent to findOneAndUpdate({ _id: id }, ...).
         *
         * If mongoose option 'useFindAndModify': set to false it uses native findOneAndUpdate() rather than deprecated findAndModify().
         * https://mongoosejs.com/docs/api.html#mongoose_Mongoose-set
         *
         * @param id value of _id to query by
         */
        findByIdAndUpdate(): DocumentQuery<T | null, T> & QueryHelpers;
        findByIdAndUpdate(id: any | number | string, update: any,
            callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;
        findByIdAndUpdate(id: any | number | string, update: any,
            options: { rawResult: true } & { upsert: true } & { new: true } & QueryFindOneAndUpdateOptions,
            callback?: (err: any, res: T) => void): DocumentQuery<T, T> & QueryHelpers;
        findByIdAndUpdate(id: any | number | string, update: any,
            options: { upsert: true, new: true } & QueryFindOneAndUpdateOptions,
            callback?: (err: any, res: mongodb.FindAndModifyWriteOpResultObject<T>) => void)
            : Query<mongodb.FindAndModifyWriteOpResultObject<T>> & QueryHelpers;
        findByIdAndUpdate(id: any | number | string, update: any,
            options: { rawResult: true } & QueryFindOneAndUpdateOptions,
            callback?: (err: any, res: mongodb.FindAndModifyWriteOpResultObject<T | null>) => void)
            : Query<mongodb.FindAndModifyWriteOpResultObject<T | null>> & QueryHelpers;
        findByIdAndUpdate(id: any | number | string, update: any,
            options: QueryFindOneAndUpdateOptions,
            callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;

        /**
         * Finds one document.
         * The conditions are cast to their respective SchemaTypes before the command is sent.
         * @param projection optional fields to return
         */
        findOne(conditions?: ConditionObject<T>,
            callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;
        findOne(conditions: ConditionObject<T>, projection: any,
            callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;
        findOne(conditions: ConditionObject<T>, projection: any, options: any,
            callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;

        /**
         * Issue a mongodb findAndModify remove command.
         * Finds a matching document, removes it, passing the found document (if any) to the callback.
         * Executes immediately if callback is passed else a Query object is returned.
         *
         * If mongoose option 'useFindAndModify': set to false it uses native findOneAndUpdate() rather than deprecated findAndModify().
         * https://mongoosejs.com/docs/api.html#mongoose_Mongoose-set
         *
         * Note: same signatures as findOneAndDelete
         *
         */
        findOneAndRemove(): DocumentQuery<T | null, T> & QueryHelpers;
        findOneAndRemove(conditions: ConditionObject<T>,
            callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;
        findOneAndRemove(conditions: ConditionObject<T>, options: { rawResult: true } & QueryFindOneAndRemoveOptions,
            callback?: (err: any, doc: mongodb.FindAndModifyWriteOpResultObject<T | null>, res: any) => void)
            : Query<mongodb.FindAndModifyWriteOpResultObject<T | null>> & QueryHelpers;
        findOneAndRemove(conditions: ConditionObject<T>, options: QueryFindOneAndRemoveOptions, callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;

        /**
         * Issues a mongodb findOneAndDelete command.
         * Finds a matching document, removes it, passing the found document (if any) to the
         * callback. Executes immediately if callback is passed.
         *
         * Note: same signatures as findOneAndRemove
         *
         */
        findOneAndDelete(): DocumentQuery<T | null, T> & QueryHelpers;
        findOneAndDelete(conditions: ConditionObject<T>,
            callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;
        findOneAndDelete(conditions: ConditionObject<T>, options: { rawResult: true } & QueryFindOneAndRemoveOptions,
            callback?: (err: any, doc: mongodb.FindAndModifyWriteOpResultObject<T | null>, res: any) => void)
            : Query<mongodb.FindAndModifyWriteOpResultObject<T | null>> & QueryHelpers;
        findOneAndDelete(conditions: ConditionObject<T>, options: QueryFindOneAndRemoveOptions, callback?: (err: any, res: T | null) => void): DocumentQuery<T | null, T> & QueryHelpers;

        /**
         * Issues a mongodb findAndModify update command.
         * Finds a matching document, updates it according to the update arg, passing any options,
         * and returns the found document (if any) to the callback. The query executes immediately
         * if callback is passed else a Query object is returned.
         *
    +    * If mongoose option 'useFindAndModify': set to false it uses native findOneAndUpdate() rather than the deprecated findAndModify().
    +    * https://mongoosejs.com/docs/api.html#mongoose_Mongoose-set
         */
        findOneAndUpdate(): DocumentQuery<T | null, T> & QueryHelpers;
        findOneAndUpdate(conditions: ConditionObject<T>, update: any,
            callback?: (err: any, doc: T | null, res: any) => void): DocumentQuery<T | null, T> & QueryHelpers;
        findOneAndUpdate(conditions: ConditionObject<T>, update: any,
            options: { rawResult: true } & { upsert: true, new: true } & QueryFindOneAndUpdateOptions,
            callback?: (err: any, doc: mongodb.FindAndModifyWriteOpResultObject<T>, res: any) => void)
            : Query<mongodb.FindAndModifyWriteOpResultObject<T>> & QueryHelpers;
        findOneAndUpdate(conditions: ConditionObject<T>, update: any,
            options: { upsert: true, new: true } & QueryFindOneAndUpdateOptions,
            callback?: (err: any, doc: T, res: any) => void): DocumentQuery<T, T> & QueryHelpers;
        findOneAndUpdate(conditions: ConditionObject<T>, update: any,
            options: { rawResult: true } & QueryFindOneAndUpdateOptions,
            callback?: (err: any, doc: mongodb.FindAndModifyWriteOpResultObject<T | null>, res: any) => void)
            : Query<mongodb.FindAndModifyWriteOpResultObject<T | null>> & QueryHelpers;
        findOneAndUpdate(conditions: ConditionObject<T>, update: any,
            options: QueryFindOneAndUpdateOptions,
            callback?: (err: any, doc: T | null, res: any) => void): DocumentQuery<T | null, T> & QueryHelpers;

        /**
         * Shortcut for validating an array of documents and inserting them into
         * MongoDB if they're all valid. This function is faster than .create()
         * because it only sends one operation to the server, rather than one for each
         * document.
         * This function does not trigger save middleware.
         * @param docs Documents to insert.
         * @param options Optional settings.
         * @param options.ordered  if true, will fail fast on the first error encountered.
         *        If false, will insert all the documents it can and report errors later.
         * @param options.rawResult if false, the returned promise resolves to the documents that passed mongoose document validation.
         *        If `false`, will return the [raw result from the MongoDB driver](http://mongodb.github.io/node-mongodb-native/2.2/api/Collection.html#~insertWriteOpCallback)
         *        with a `mongoose` property that contains `validationErrors` if this is an unordered `insertMany`.
         */
        insertMany(docs: any[], callback?: (error: any, docs: T[]) => void): Promise<T[]>;
        insertMany(docs: any[], options?: { ordered?: boolean, rawResult?: boolean } & ModelOptions, callback?: (error: any, docs: T[]) => void): Promise<T[]>;
        insertMany(doc: any, callback?: (error: any, doc: T) => void): Promise<T>;
        insertMany(doc: any, options?: { ordered?: boolean, rawResult?: boolean } & ModelOptions, callback?: (error: any, doc: T) => void): Promise<T>;

        /**
         * Populates document references.
         * @param docs Either a single document or array of documents to populate.
         * @param options A hash of key/val (path, options) used for population.
         * @param callback Optional callback, executed upon completion. Receives err and the doc(s).
         */
        populate(docs: any[], options: ModelPopulateOptions | ModelPopulateOptions[],
            callback?: (err: any, res: T[]) => void): Promise<T[]>;
        populate<T>(docs: any, options: ModelPopulateOptions | ModelPopulateOptions[],
            callback?: (err: any, res: T) => void): Promise<T>;

        /** Removes documents from the collection. */
        remove(conditions: ConditionObject<T>, callback?: (err: any) => void): Query<mongodb.DeleteWriteOpResultObject['result'] & { deletedCount?: number }> & QueryHelpers;
        deleteOne(conditions: ConditionObject<T>, callback?: (err: any) => void): Query<mongodb.DeleteWriteOpResultObject['result'] & { deletedCount?: number }> & QueryHelpers;
        deleteMany(conditions: ConditionObject<T>, callback?: (err: any) => void): Query<mongodb.DeleteWriteOpResultObject['result'] & { deletedCount?: number }> & QueryHelpers;

        /**
         * Same as update(), except MongoDB replace the existing document with the given document (no atomic operators like $set).
         * This function triggers the following middleware: replaceOne
         */
        replaceOne(conditions: ConditionObject<T>, replacement: any, callback?: (err: any, raw: any) => void): Query<any> & QueryHelpers;

        /**
         * Updates documents in the database without returning them.
         * All update values are cast to their appropriate SchemaTypes before being sent.
         */
        update(conditions: ConditionObject<T>, doc: any,
            callback?: (err: any, raw: any) => void): Query<any> & QueryHelpers;
        update(conditions: ConditionObject<T>, doc: any, options: ModelUpdateOptions,
            callback?: (err: any, raw: any) => void): Query<any> & QueryHelpers;
        updateOne(conditions: ConditionObject<T>, doc: any,
            callback?: (err: any, raw: any) => void): Query<any> & QueryHelpers;
        updateOne(conditions: ConditionObject<T>, doc: any, options: ModelUpdateOptions,
            callback?: (err: any, raw: any) => void): Query<any> & QueryHelpers;
        updateMany(conditions: ConditionObject<T>, doc: any,
            callback?: (err: any, raw: any) => void): Query<any> & QueryHelpers;
        updateMany(conditions: ConditionObject<T>, doc: any, options: ModelUpdateOptions,
            callback?: (err: any, raw: any) => void): Query<any> & QueryHelpers;

        /** Creates a Query, applies the passed conditions, and returns the Query. */
        where(path: FieldKeys<T>, val?: any): Query<any> & QueryHelpers;
    }

    export interface Document extends MongooseDocument, NodeJS.EventEmitter, ModelProperties {
        /**
         * Removes this document from the db.
         * @param fn optional callback
         */
        remove(fn?: (err: any, product: this) => void): Promise<this>;

        /**
         * Saves this document.
         * @param options options optional options
         * @param options.safe overrides schema's safe option
         * @param options.validateBeforeSave set to false to save without validating.
         * @param fn optional callback
         */
        save(options?: SaveOptions, fn?: (err: any, product: this) => void): Promise<this>;
        save(fn?: (err: any, product: this) => void): Promise<this>;
        model<T>(ctor: Type<T> | string): ModelType<T>;
        /**
         * Version using default version key. See http://mongoosejs.com/docs/guide.html#versionKey
         * If you're using another key, you will have to access it using []: doc[_myVersionKey]
         */
        __v?: number;
    }
}
const oldModel = Model.prototype.model;
Model.prototype.model = function model<T>(ctor: Type<T> | string, schema?: Schema<any> | undefined, collection?: string | undefined) {
    if (typeof ctor == "string") {
        return oldModel.bind(this)(ctor, schema, collection);
    }
    else {
        return oldModel.bind(this)(ctor.name, schema, collection);
    }
}
