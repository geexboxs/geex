import { ModelType } from "@typegoose/typegoose/lib/types";
import { promises } from "dns";
import { request } from "express";
import { ClientSession, Document } from "mongoose";
import { Session, SessionStore } from "./models/session.model";
import { UserModelToken } from "../../shared/tokens";
import passport = require("passport");
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { I18N } from "../../shared/utils/i18n";
import ioredis = require("ioredis");
import { Enforcer } from "casbin";
import { User } from "../account/models/user.model";
import { InjectModel } from '@nestjs/mongoose';
import * as bcryptjs from "bcryptjs";
import { AccessControl } from "@geexbox/accesscontrol";
import { JwtService } from "@nestjs/jwt";
import { Resolver, Mutation, Args } from "@nestjs/graphql";
import { Authorized } from "type-graphql";
import { Ctx } from "type-graphql/dist/decorators";
import { PasswordHasher } from "../account/utils/password-hasher";
import { Inject, ExecutionContext } from "@nestjs/common";

@Resolver((of) => Session)
export class AuthenticationResolver {
    constructor(
        @InjectModel(nameof(User))
        private userModel: ModelType<User>,
        @Inject(PasswordHasher)
        private passwordHasher: PasswordHasher,
        @Inject(SessionStore)
        private sessionStore: SessionStore,
        @Inject(AccessControl)
        private ac: AccessControl,
    ) { }
    @Mutation(() => Boolean)
    @Authorized()
    public async signOut(@Ctx() context: ExecutionContext) {
        // await this.sessionStore.del(context.session.getUser().id);
        // context.session.logout();
        return true;
    }

    @Mutation(() => Session)
    public async externalAuthenticate(@Args("provider") provider: string, @Args("userIdentifier") userIdentifier: string) {
        // todo:
        throw Error("todo");
    }

    @Mutation(() => Session)
    public async authenticate(@Args("userIdentifier") userIdentifier: string, @Args("password") password: string, @Ctx() context: ExecutionContext) {
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
    public async impersonate(@Args("userIdentifier") userIdentifier: string) {
        // todo:
        throw Error("todo");
    }
}
