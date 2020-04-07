import { Inject, Injector } from "@graphql-modules/di";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { promises } from "dns";
import { request } from "express";
import { ClientSession, Document } from "mongoose";
import { Arg, Args, Authorized, FieldResolver, Mutation, Query, Resolver, ResolverInterface, Root, UseMiddleware, ID, Ctx } from "type-graphql";
import { Session, SessionStore } from "./models/session.model";
import { UserModelToken } from "../../shared/tokens";
import passport = require("passport");
import { IGeexContext, IUserContext } from "../../types";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { I18N } from "../../shared/utils/i18n";
import ioredis = require("ioredis");
import { Enforcer } from "casbin";
import { EmailSender } from "../../shared/utils/email-sender";
import { permission } from "../session/rules/permission.rule";
import { User } from "../account/models/user.model";
import { InjectModel } from '@nestjs/mongoose';
import * as bcryptjs from "bcryptjs";
import { AccessControl } from "@geexbox/accesscontrol";
import { JwtService } from "@nestjs/jwt";

@Resolver((of) => Session)
export class SessionResolver {
    constructor(
        @InjectModel(nameof(User))
        private userModel: ModelType<User>,
        @Inject(ioredis)
        private redis: ioredis.Redis,
        @Inject(SessionStore)
        private sessionStore: SessionStore,
        @Inject(AccessControl)
        private ac: AccessControl,
    ) { }
    @Mutation(() => Boolean)
    @Authorized()
    public async signOut(@Ctx() context: IGeexContext) {
        await this.sessionStore.del(context.session.getUser().id);
        // context.session.logout();
        return true;
    }

    @Mutation(() => Session)
    public async externalAuthenticate(@Arg("provider") provider: string, @Arg("userIdentifier") userIdentifier: string) {
        // todo:
        throw Error("todo");
    }

    @Mutation(() => Session)
    public async authenticate(@Arg("userIdentifier") userIdentifier: string, @Arg("password") password: string, @Ctx() context: IGeexContext) {
        const user = await this.userModel.findOne({ username: userIdentifier })
        if (!user) {
            throw Error(I18N.message.userIdentifierOrPasswordIncorrect);
        }
        const sessionCache = await this.sessionStore.createOrRefresh(user.toUserContext());
        const valid = await bcryptjs.compare(password, user.passwordHash);
        if (!valid) {
            throw Error('Email or password incorrect');
        }
        return sessionCache;
    }

    @Mutation(() => Boolean)
    public async impersonate(@Arg("userIdentifier") userIdentifier: string) {
        // todo:
        throw Error("todo");
    }
}
