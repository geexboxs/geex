import { Inject } from "@graphql-modules/di";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { promises } from "dns";
import { request } from "express";
import { ClientSession, Document } from "mongoose";
import { Arg, Args, Authorized, FieldResolver, Mutation, Query, Resolver, ResolverInterface, Root, UseMiddleware } from "type-graphql";
import { AuthMiddleware } from "../../shared/auth/auth.middleware";
import { UserModelToken } from "./tokens";
import { User } from "./user.model";

@Resolver((of) => User)
export class UserResolver {

    constructor(
        @Inject(UserModelToken)
        private userModel: ModelType<User>,

    ) { }

    @Query((returns) => [User])
    public async users() {
        const result = await this.userModel.find({}).exec();
        return result;
    }

    @Query((returns) => User)
    @UseMiddleware(AuthMiddleware)
    public async user(id: string) {
        const result = await this.userModel.findOne({ _id: id }).exec();
        return result;
    }
}
