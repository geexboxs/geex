import { Inject, Injector } from "@graphql-modules/di";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { promises } from "dns";
import { request } from "express";
import { User } from "./models/user.model";
import { UserModelToken } from "../../shared/tokens";
import { Session, SessionStore } from "../authentication/models/session.model";
import { VerifyType } from "./models/verify-type";
import { RegisterInput } from "./models/register.input";
import passport = require("passport");
import { IGeexContext } from "../../types";
import { PasswordHasher } from "./utils/password-hasher";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { I18N } from "../../shared/utils/i18n";
import ioredis = require("ioredis");
import { EmailSender } from "../../shared/utils/email-sender";
import { permission } from "../authentication/rules/permission.rule";
import { Resolver, Mutation, ID, Args, Query } from "@nestjs/graphql";
import { REQUEST } from "@nestjs/core";
import { ExecutionContext, Optional } from "@nestjs/common";
import { InjectModel } from '@nestjs/mongoose';

@Resolver((of) => User)
export class AccountResolver {
    constructor(
        @InjectModel(nameof(User))
        private userModel: ModelType<User>,
        @Inject(PasswordHasher)
        private passwordHasher: PasswordHasher,
        @Optional()
        @Inject(EmailSender)
        private emailSender: EmailSender,
        @Inject(REQUEST)
        private request: ExecutionContext,
    ) { }

    @Mutation(() => ID)
    public async register(@Args("registerInput") { username: username, password }: RegisterInput) {
        const newUser = new User(username, this.passwordHasher.hash(password));
        const result = await this.userModel.create(newUser);
        return result.id;
    }

    @Mutation(() => Boolean)
    public async verify(@Args("code") code: string) {
        // todo:
        throw Error("todo");
    }
    @Mutation(() => Boolean)
    public async resetPassword(@Args("code") code: string, @Args("newPassword") newPassword: string) {
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
    public async sendVerifyCode(@Args({ name: 'type', type: () => VerifyType }) type: VerifyType, @Args("target") target: string) {
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
    public async sendResetPasswordCode(@Args({ name: 'type', type: () => VerifyType }) type: VerifyType, @Args("target") target: string) {
        // todo:
        throw Error("todo");
    }

    @Mutation(() => Boolean)
    public async changePassword(@Args("oldPassword") oldPassword: string, @Args("newPassword") newPassword: string) {
        let userDoc = await this.userModel.findById(this.request.switchToHttp().getRequest().session.getUser().id).exec();
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
    public async setTwoFactor(@Args("key") key: string, @Args("code") code: string) {
        // todo:
        throw Error("todo");
    }
    @Mutation(() => Boolean)
    public async unsetTwoFactor(@Args("code") code: string) {
        // todo:
        throw Error("todo");
    }


    @Query(() => String)
    public async twoFactorKey() {
        // todo:
        throw Error("todo");
    }
}
