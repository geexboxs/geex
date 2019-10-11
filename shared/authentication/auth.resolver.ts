
import { User } from "../../domain/models/user.model";

import { Inject } from "@graphql-modules/di";

import { UserModelToken } from "./auth.module";

import { ModelType } from "@typegoose/typegoose/lib/types";


import { SchemaDirective } from "../../graphql-patch";

import { Authorized, Resolver, Query, Mutation } from "type-graphql";
import { AuthTokenScalar } from "./types/scalars";
import passport = require("passport");

@Resolver()
export class AuthResolver {

    constructor(
        @Inject(UserModelToken)
        private userModel: ModelType<User>,

    ) { }

    @Mutation(returns => AuthTokenScalar)
    async authenticate(username: string, password: string) {
        return passport.authenticate("local");
    }

    @Query(returns => AuthTokenScalar)
    async token(username: string, password: string) {
        return passport.authenticate("local");
    }
}
