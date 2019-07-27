import { prop, Typegoose, pre, arrayProp, Ref, instanceMethod, ModelType, InstanceType, plugin } from 'typegoose';
import { Address } from './address.model';
import { Model, Types, Document, Schema } from 'mongoose';
import { ObjectType, Field, Authorized, UseMiddleware } from 'type-graphql';
import { CreateUserInput } from '../../utils/CreateUserInput';
import mongooseAutopopulate = require("mongoose-autopopulate");
import { isObject } from 'typegoose/lib/utils';
import { ObjectId } from 'mongodb';

@pre<User>('save', function (next) {
    if (!this) {
        next();
    }
    if (this.createAt === undefined) {
        this.createAt = new Date();
    }
    this.updateAt = new Date();
    next();
})
@ObjectType()
export class User extends Typegoose {
    async Init(input: CreateUserInput) {
        this.name = input.name;
        this.surname = input.surname;
        if (input.addresses) {
            this["_addresses"] = (await Promise.all(input.addresses.map(async x => {
                var address = new Address({ address: x });
                address = await User.addressRepo.create(address);
                return address;
            })));
        }
    }
    /**
     *
     */
    constructor() {
        super();
    }
    static repo: ModelType<User>
    static addressRepo: ModelType<Address>
    @Field(returns => String)
    _id!: Schema.Types.ObjectId;
    @prop()
    @Field()
    name!: string;
    @prop()
    @Field()
    surname!: string;
    @Field()
    @prop()
    get fullName(): string {
        return `${this.name} ${this.surname}`;
    }
    @arrayProp({ itemsRef: Address })
    protected _addresses!: Ref<Address>[];
    @Field(type => [Address])
    public get addresses(): Promise<Address[]> {
        return (async () => {
            if (this["_doc"]._addresses[0] instanceof ObjectId) {
                this["_doc"]._addresses = (await User.repo.populate<User>(this, { path: "_addresses" }))._addresses;
            }
            return this["_doc"]._addresses as Address[];
        })()
    }

    @prop()
    @Field()
    createAt!: Date;
    @prop()
    @Field()
    updateAt!: Date;
    // @instanceMethod
    // async addAddress(address: Address): Promise<void> {
    //     try {
    //         await User.repo.updateOne({ _id: this._id }, { $push: { addresses: { $each: [address] } } }).exec();
    //     }
    //     catch (e) {
    //         console.log(e);
    //     }
    // }
    @prop()
    @Field(type => [String])
    roles!: string[];
}
export const UserModel = new User().getModelForClass(User);
export const UserModelToken = Symbol("UserModel");


