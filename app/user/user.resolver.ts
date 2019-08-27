import { Resolver, Query, Mutation, Authorized, Arg, FieldResolver, Root, Args, ResolverInterface, UseMiddleware } from "type-graphql";
import { Inject } from "@graphql-modules/di";
import { promises } from "dns";
import { InstanceType, ModelType } from "typegoose";
import { ClientSession, Document } from "mongoose";
import { request } from "express";
import { User } from "../../domain/models/user.model";
import { UserModelToken } from "../../domain/domain.module";

@Resolver(of => User)
export class UserResolver {

    constructor(
        @Inject(UserModelToken)
        private userModel: ModelType<User>,

    ) { }

    @Query(returns => [User])
    async users() {
        let result = await this.userModel.find({}).exec();
        return result;
    }

    @Query(returns => User)
    @Authorized()
    async user(id: string) {
        let result = await this.userModel.findOne({ _id: id }).exec();
        return result;
    }
}
