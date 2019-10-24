import { Inject, Injector } from "@graphql-modules/di";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { promises } from "dns";
import { request } from "express";
import { ClientSession, Document } from "mongoose";
import { Arg, Args, Authorized, FieldResolver, Mutation, Query, Resolver, ResolverInterface, Root, UseMiddleware, ID, Ctx } from "type-graphql";
import { User } from "./models/user.model";
import { UserModelToken } from "../../shared/tokens";
import { Session } from "./models/session.model";
import { VerifyType } from "./models/verify-type";
import { RegisterInput } from "./models/register.input";
import passport = require("passport");
import { IGeexContext } from "../../types";
import { PasswordHasher } from "./utils/password-hasher";
import { i18n } from "../../shared/utils/i18n";

@Resolver((of) => User)
export class UserResolver {

    constructor(
        @Inject(UserModelToken)
        private userModel: ModelType<User>,

    ) { }

    @Mutation(() => ID)
    public async register(@Arg("registerInput") { username, password }: RegisterInput, @Ctx<IGeexContext>("injector") injector: Injector) {
        const newUser = User.create(username, password, injector.get(PasswordHasher));
        const result = await this.userModel.create(newUser);
        return result.id;
    }

    @Mutation(() => Boolean)
    public async verify(@Arg("code") code: string) {
        // todo:
        throw Error("todo");
    }
    @Mutation(() => Session)
    public async resetPassword(@Arg("code") code: string, @Arg("newPassword") newPassword: string) {
        // todo:
        throw Error("todo");
    }

    /**
     * todo: validate target
     *
     * @param {VerifyType} type
     * @param {string} target
     * @memberof UserResolver
     */
    @Mutation(() => Boolean)
    public async sendVerifyCode(@Arg("type", () => VerifyType) type: VerifyType, @Arg("target") target: string) {
        switch (type) {
            case VerifyType.Email:
                
                break;
            case VerifyType.Sms:

                break;
            default:
                throw Error(i18n("InvalidInput"));
        }
    }

    /**
     * todo: validate target
     *
     * @param {VerifyType} type
     * @param {string} target
     * @memberof UserResolver
     */
    @Mutation(() => Boolean)
    public async sendResetPasswordCode(@Arg("type", () => VerifyType) type: VerifyType, @Arg("target") target: string) {
        // todo:
        throw Error("todo");
    }

    @Mutation(() => Boolean)
    public async changePassword(@Arg("oldPassword") oldPassword: string, @Arg("newPassword") newPassword: string) {
        // todo:
        throw Error("todo");
    }
    @Mutation(() => Boolean)
    public async setTwoFactor(@Arg("key") key: string, @Arg("code") code: string) {
        // todo:
        throw Error("todo");
    }
    @Mutation(() => Boolean)
    public async unsetTwoFactor(@Arg("code") code: string) {
        // todo:
        throw Error("todo");
    }
    @Mutation(() => Boolean)
    public async impersonate(@Arg("userIdentifier") userIdentifier: string) {
        // todo:
        throw Error("todo");
    }
    @Mutation(() => Session)
    public async refreshToken(@Arg("refreshToken") refreshToken: string) {
        // todo: refreshToken need to check ip change
        throw Error("todo");
    }
    @Mutation(() => Session)
    public async externalAuthenticate(@Arg("provider") provider: string, @Arg("userIdentifier") userIdentifier: string) {
        // todo:
        throw Error("todo");
    }

    @Mutation(() => Session)
    public async authenticate(@Arg("userIdentifier") userIdentifier: string, @Arg("password") password: string) {
        // todo:
        throw Error("todo");
    }
    @Query(() => String)
    public async twoFactorKey() {
        // todo:
        throw Error("todo");
    }
}
