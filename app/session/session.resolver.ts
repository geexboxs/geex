import { Inject, Injector } from "@graphql-modules/di";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { promises } from "dns";
import { request } from "express";
import { ClientSession, Document } from "mongoose";
import { Arg, Args, Authorized, FieldResolver, Mutation, Query, Resolver, ResolverInterface, Root, UseMiddleware, ID, Ctx } from "type-graphql";
import { Session, SessionStore } from "./models/session.model";
import { UserModelToken } from "../../shared/tokens";
import passport = require("passport");
import { IGeexContext } from "../../types";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { I18N } from "../../shared/utils/i18n";
import ioredis = require("ioredis");
import { Enforcer } from "casbin";
import { EmailSender } from "../../shared/utils/email-sender";
import { permission } from "../session/rules/permission.rule";

@Resolver((of) => Session)
export class SessionResolver {
    constructor(
        @Inject(UserModelToken)
        private userModel: ModelType<Session>,
        @Inject(ioredis)
        private redis: ioredis.Redis,
        @Inject(SessionStore)
        private sessionStore: SessionStore,
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
        const { user } = await context.session.authenticate("local", { username: userIdentifier, password });
        if (user) {
            const sessionCache = await this.sessionStore.createOrRefresh(user);
            return sessionCache;
        }
        throw Error(I18N.message.userIdentifierOrPasswordIncorrect);
    }

    @Mutation(() => Boolean)
    public async impersonate(@Arg("userIdentifier") userIdentifier: string) {
        // todo:
        throw Error("todo");
    }
}
