import { prop } from "@typegoose/typegoose";
import { DateTimeResolver, IPv4Resolver } from "graphql-scalars";
import { IUserContext } from "../../../types";
import ioredis = require("ioredis");
import { Jwt, JwtPayload } from "../../../shared/utils/jwt";
import { Inject } from "@graphql-modules/di";
import json5 = require("json5");
import { JwtService } from "@nestjs/jwt";
import { ObjectType, Field, ID } from "@nestjs/graphql";

@ObjectType()
export class Session {

    @Field(() => ID)
    public get userId() {
        return this.user.id;
    }
    @Field(() => String)
    public accessToken!: string;
    @Field(() => DateTimeResolver)
    public expireAt!: Date;
    user!: IUserContext;
    /**
     *
     */
    constructor(init: Partial<Session>) {
        Object.assign(this, init);
    }
}

export class SessionStore {
    ttl: number;
    /**
     *
     */

    constructor(@Inject(ioredis) private redis: ioredis.Redis,
        private jwtService: JwtService,
    ) {
        this.ttl = 3600 * 24 * 10;
    }
    async del(userId: any) {
        await this.redis.del(`session:${userId}`);
    }
    async createOrRefresh(user: IUserContext) {
        const session = new Session({
            accessToken: this.jwtService.sign({ ...user}),
            user,
            expireAt: new Date().add({ seconds: this.ttl }),
        });
        await this.redis.setex(`session:${user.id}`, this.ttl, json5.stringify(session));
        return session;
    }
    async get(userId: string): Promise<Session | null> {
        const sessionStr = await this.redis.get(`session:${userId}`);
        if (sessionStr) {
            const session = json5.parse(sessionStr) as Session;
            return session;
        }
        return null;
    }
    clear() {
        // todo:
        throw Error("todo");
    }
}

export interface ISessionStoreOptions {
    ttl: number;
}
