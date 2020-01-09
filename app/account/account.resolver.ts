import { Inject, Injector } from "@graphql-modules/di";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { promises } from "dns";
import { request } from "express";
import { ClientSession, Document } from "mongoose";
import { Arg, Args, Authorized, FieldResolver, Mutation, Query, Resolver, ResolverInterface, Root, UseMiddleware, ID, Ctx } from "type-graphql";
import { User } from "./models/user.model";
import { UserModelToken } from "../../shared/tokens";
import { Session, SessionStore } from "../session/models/session.model";
import { VerifyType } from "./models/verify-type";
import { RegisterInput } from "./models/register.input";
import passport = require("passport");
import { IGeexContext } from "../../types";
import { PasswordHasher } from "./utils/password-hasher";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { I18N } from "../../shared/utils/i18n";
import ioredis = require("ioredis");
import { Enforcer } from "casbin";
import { EmailSender } from "../../shared/utils/email-sender";
import { permission } from "../session/rules/permission.rule";

@Resolver((of) => User)
export class AccountResolver {
    constructor(
        @Inject(UserModelToken)
        private userModel: ModelType<User>,
        @Inject(PasswordHasher)
        private passwordHasher: PasswordHasher,
        @Inject(EmailSender)
        private emailSender: EmailSender,

    ) { }

    @Mutation(() => ID)
    public async register(@Arg("registerInput") { username, password }: RegisterInput, @Ctx<IGeexContext>("injector") injector: Injector) {
        const newUser = new User(username, injector.get(PasswordHasher).hash(password));
        const result = await this.userModel.create(newUser);
        return result.id;
    }

    @Mutation(() => Boolean)
    public async verify(@Arg("code") code: string) {
        // todo:
        throw Error("todo");
    }
    @Mutation(() => Boolean)
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
                this.emailSender.send([{ name: target, address: target }], "", "text")
                break;
            case VerifyType.Sms:

                break;
            default:
                throw Error(I18N.message.InvalidParams);
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
    @Authorized()
    public async changePassword(@Arg("oldPassword") oldPassword: string, @Arg("newPassword") newPassword: string, @Ctx() context: IGeexContext) {
        let userDoc = await this.userModel.findById(context.session.getUser().id).exec();
        if (userDoc === null) {
            throw new Error(I18N.message.userNotFound);
        }
        if (userDoc.passwordHash === this.passwordHasher.hash(oldPassword)) {
            userDoc.passwordHash = this.passwordHasher.hash(newPassword);
            userDoc = await userDoc.save();
            return Boolean(userDoc && userDoc.passwordHash === this.passwordHasher.hash(newPassword));
        } else {
            throw new Error(I18N.message.passwordIncorrect);
        }
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
    
    
    @Query(() => String)
    public async twoFactorKey() {
        // todo:
        throw Error("todo");
    }
}
