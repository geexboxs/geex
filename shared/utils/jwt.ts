import * as _ from "lodash";
import { Netmask } from "netmask";
import { IGeexContext, IUserContext } from "../../types";
import { Request } from "express";
import * as jwt from "jsonwebtoken";

export class Jwt {

    constructor(private secret: string, private iss: string) {

    }
    sign(payload: JwtPayload) {
        payload.iss = payload.iss || this.iss;
        payload.alg = payload.alg || "HS256";
        return jwt.sign({ ...payload }, this.secret);
    }

    /**
     *
     * verify token is valid and not expire
     *
     * @param {string} payload
     * @param {string} userId
     * @param {("accessToken" | "refreshToken")} tokenType
     * @returns 
     * @memberof Jwt
     */
    verify(payload: string, userId: string, tokenType: "accessToken" | "refreshToken", userIp: string) {
        const preCondition = jwt.verify(payload, this.secret, {
            issuer: this.iss,
            audience: tokenType,
            subject: userId,
        });
        if (preCondition) {
            const payloadObj = jwt.decode(payload)! as Partial<JwtPayload>;
            return (payloadObj.exp === undefined || payloadObj.exp > new Date().epoch());
        }
        return false;
    }
}

export class JwtPayload {
    name: string;
    sub: string;
    role: string;
    email: string;
    // tslint:disable-next-line: variable-name
    phone_number: string;
    exp: number;
    iat: number;
    picture: string;

    /**
     * Creates an instance of JwtPayload.
     * @param {(Pick<IGeexContext, "session" | "user">)} context
     * @param {("accessToken" | "refreshToken")} aud
     * @param {string} [iss]
     * @param {string} [ipr]
     * @param {string} [alg]
     * @memberof JwtPayload
     */
    constructor(
        user: IUserContext,
        public aud: "accessToken" | "refreshToken",
        public iss?: string,
        public alg?: string) {
        this.name = user.username;
        this.sub = user.id;
        this.role = user.roles.join(",");
        this.email = user.email;
        this.phone_number = user.phone;
        this.picture = user.avatarUrl;
        this.exp = new Date().add({ days: 15 }).epoch();
        this.iat = new Date().add({ hours: 1 }).epoch();
    }
}
