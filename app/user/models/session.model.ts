import { ObjectType, Field, ID } from "type-graphql";
import { prop } from "@typegoose/typegoose";
import { DateTimeResolver, IPv4Resolver } from "graphql-scalars";
import { IUserContext } from "../../../types";
import * as ioredis from "ioredis";
import { Jwt, JwtPayload } from "../../../shared/utils/jwt";
import { Inject } from "@graphql-modules/di";
import * as json5 from "json5";

@ObjectType()
export class Session {

    @Field(() => ID)
    public get userId() {
        return this.user.id;
    }
    @Field(() => String)
    public refreshToken!: string;
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
    public expired() {
        return this.expireAt < new Date();
    }
}

export class SessionStore {
    ttl: number;
    /**
     *
     */

    constructor(@Inject(ioredis.default) private redis: ioredis.Redis, @Inject(Jwt) private jwt: Jwt) {
        this.ttl = 3600 * 24 * 10;
    }
    async createOrRefresh(user: IUserContext) {
        const session = new Session({
            accessToken: this.jwt.sign(new JwtPayload(user, "accessToken")),
            refreshToken: this.jwt.sign(new JwtPayload(user, "refreshToken")),
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
            if (session && !Session.prototype.expired.call(session)) {
                return session;
            } else {
                await this.redis.del(`session:${userId}`);
                return null;
            }
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
