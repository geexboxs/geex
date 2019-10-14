
import { User } from "../../app/user/user.model";


import { Authorized, Resolver, Query, Mutation, UseMiddleware } from "type-graphql";
import passport = require("passport");
import { AuthMiddleware } from "./auth.middleware";
import { Inject, Injectable } from "@graphql-modules/di";
import { ReturnModelType } from "@typegoose/typegoose";
import { UserModelToken } from "./tokens";

@Resolver()
@Injectable()
export class AuthResolver {

    constructor(
        @Inject(UserModelToken)
        public userModel: ReturnModelType<typeof User>,

    ) { }

    @Mutation(returns => String)
    async authenticate(): Promise<string> {
        return "1"
    }

    @Query(returns => String)
    @UseMiddleware(AuthMiddleware)
    async token(): Promise<string> {
        return "0"
    }
}
