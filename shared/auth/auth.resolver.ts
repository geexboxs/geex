
import { User } from "../../app/user/user.model";

import { Inject, Injectable } from "@graphql-modules/di";
import { ReturnModelType } from "@typegoose/typegoose";
import passport = require("passport");
import { Authorized, Mutation, Query, Resolver, UseMiddleware } from "type-graphql";
import { AuthMiddleware } from "./auth.middleware";
import { UserModelToken } from "./tokens";

@Resolver()
@Injectable()
export class AuthResolver {

    constructor(
        @Inject(UserModelToken)
        public userModel: ReturnModelType<typeof User>,

    ) { }

    @Mutation((returns) => String)
    public async authenticate(): Promise<string> {
        return "1";
    }

    @Query((returns) => String)
    @UseMiddleware(AuthMiddleware)
    public async token(): Promise<string> {
        return "0";
    }
}
