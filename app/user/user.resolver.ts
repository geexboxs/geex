import { LogResponse } from "../../shared/logging/logResponse.decorator";
import { Resolver, Query, Mutation, Authorized, Arg, FieldResolver, Root, Args, ResolverInterface, UseMiddleware } from "type-graphql";
import { CreateUserInput } from '../../utils/CreateUserInput'
import { Inject } from "@graphql-modules/di";
import { promises } from "dns";
import { InstanceType, ModelType } from "typegoose";
import { ClientSession, Document } from "mongoose";
import { IResolver } from "../../utils/types";
import { StudentInput } from '../../utils/CreateStudentInput';
import { LogInfo } from "../custom-middlewares/middlewares/LogInfo";
import { request } from "express";
import { User } from "../../domain/models/user.model";
import { UserModelToken } from "../../domain/domain.module";


@Resolver(of => User)
export class UserResolver {

    constructor(
        @Inject(UserModelToken)
        private userModel: ModelType<User>,
        // @Inject(StudentModelToken)
        // private studentModel: ModelType<Student>,

    ) { }

    // @UseMiddleware(LogInfo)
    @Query(returns => [User])
    @LogResponse()
    async users() {
        let result = await this.userModel.find({}).exec();
        return result;
    }

    @Query(returns => User)
    @Authorized()
    @LogResponse()
    async user(id: string) {
        let result = await this.userModel.findOne({ _id: id }).exec();
        return result;
    }

    @Authorized()
    @Mutation(returns => User)
    // @Transaction()
    async addUser(@Args() CreateUserInput: CreateUserInput): Promise<InstanceType<User>> {
        // let curSession!: ClientSession;
        // await this.userModel.db.startSession(undefined, async (err, session) => {
        //     curSession = session;
        //     session.startTransaction();
        // });
        let newUser = new User();
        await newUser.init(CreateUserInput)
        let result = await this.userModel.create(newUser);
        // await curSession.commitTransaction();
        return result;
    }

    @Authorized('ADMIN')
    @Mutation(returns => Boolean)
    async removeUser(@Arg("id") id: string): Promise<boolean> {
        let result = await this.userModel.remove({ _id: id }).exec();
        if (result.n) {
            return true;
        };
        return false;
    }



    // @FieldResolver(returns => [Address])
    // async addresses(@Root() user: InstanceType<User>): Promise<Address[]> {
    //     user = await user.execPopulate();
    //     return user.addresses as Address[]
    // }

}
