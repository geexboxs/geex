import { Inject } from "@graphql-modules/di";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { promises } from "dns";
import { request } from "express";
import { ClientSession, Document } from "mongoose";
import { Arg, Args, Authorized, FieldResolver, Mutation, Query, Resolver, ResolverInterface, Root, UseMiddleware } from "type-graphql";
import { User } from "./user.model";
import { UserModelToken } from "../../shared/tokens";

@Resolver((of) => User)
export class UserResolver {

    constructor(
        @Inject(UserModelToken)
        private userModel: ModelType<User>,

    ) { }

    @Query((returns) => [User])
    public async users() {
        const result = await this.userModel.find().exec();
        return result;
    }

    @Query((returns) => User)
    @Authorized()
    public async user(@Arg("id")id: string) {
        const result = await this.userModel.findOne({ _id: id }).exec();
        return result;
    }
}
