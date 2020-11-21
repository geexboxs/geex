import { ModelType } from "@typegoose/typegoose/lib/types";
import { Session, SessionStore } from "./models/session.model";
import { User } from "../account/models/user.model";
import { InjectModel } from '@nestjs/mongoose';
import { AccessControl } from "@geexbox/accesscontrol";
import { Resolver, Mutation, Args } from "@nestjs/graphql";
import { Authorized } from "type-graphql";
import { Ctx } from "type-graphql/dist/decorators";
import { PasswordHasher } from "../account/utils/password-hasher";
import { Inject, ExecutionContext } from "@nestjs/common";
import { I18N } from '@geex/api-shared';

@Resolver(() => Session)
export class AuthenticationResolver {
  constructor(
    @Inject(User)
    private userModel: ModelType<User>,
    @Inject(PasswordHasher)
    private passwordHasher: PasswordHasher,
    @Inject(SessionStore)
    private sessionStore: SessionStore,
  ) { }
  @Mutation(() => Boolean)
  @Authorized()
  public async signOut() {
    // await this.sessionStore.del(context.session.getUser().id);
    // context.session.logout();
    return true;
  }

  @Mutation(() => Session)
  public async externalAuthenticate() {
    // todo:
    throw Error("todo");
  }

  @Mutation(() => Session)
  public async authenticate(@Args("userIdentifier") userIdentifier: string, @Args("password") password: string) {
    const user = await this.userModel.findOne({ username: userIdentifier })
    if (!user) {
      throw Error(I18N.message.userIdentifierOrPasswordIncorrect);
    }
    const sessionCache = await this.sessionStore.createOrRefresh(await user.toContextUser());
    const valid = this.passwordHasher.verify(password, user.passwordHash);
    if (!valid) {
      throw Error(I18N.message.userIdentifierOrPasswordIncorrect);
    }
    return sessionCache;
  }

  @Mutation(() => Boolean)
  public async impersonate() {
    // todo:
    throw Error("todo");
  }
}
