import { ModelType } from "@typegoose/typegoose/lib/types";
import { request } from "express";
import { User } from "./models/user.model";
import { Session, SessionStore } from "../authentication/models/session.model";
import { VerifyType } from "./models/verify-type";
import passport = require("passport");
import { PasswordHasher } from "./utils/password-hasher";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import ioredis = require("ioredis");
import { Resolver, Mutation, ID, Args, Query } from "@nestjs/graphql";
import { REQUEST } from "@nestjs/core";
import { ExecutionContext, Optional, Inject } from "@nestjs/common";
import { InjectModel } from '@nestjs/mongoose';
import { CurrentUow, EmailSender, I18N, UnitOfWork, Uow } from '@geex/api-shared';
import { RegisterInput } from "@geex/contracts";
import { CommandBus } from '@nestjs/cqrs';
import { SendUserRegisteredEmailCommand } from './commands/send-user-registered-email.command';

@Resolver((of) => User)
export class AccountResolver {
  constructor(
    @Inject(User)
    private userModel: ModelType<User>,
    @Inject(PasswordHasher)
    private passwordHasher: PasswordHasher,
    @Optional()
    @Inject(EmailSender)
    private emailSender: EmailSender,
    @Inject(REQUEST)
    private request: ExecutionContext,
    private commandBus: CommandBus
  ) { }

  @Mutation(() => ID)
  @Uow()
  public async register(@Args("registerInput") { username, password }: RegisterInput, @CurrentUow() uow: UnitOfWork) {
    const newUser = new User().init(username, this.passwordHasher.hash(password));
    const [result] = await this.userModel.create([newUser], { session:uow.mongoSession });
    await this.commandBus.execute(new SendUserRegisteredEmailCommand(result.id))
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
    let userDoc = await this.userModel.findById(this.request.getUser().userId).exec();
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
