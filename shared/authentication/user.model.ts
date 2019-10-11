import { prop } from "@typegoose/typegoose";

export class User {
    /**
     *
     */
    constructor({ username, passwordHash, phone, roles }: Readonly<User>) {
        this.username = username;
        this.passwordHash = passwordHash;
        this.roles = roles;
        this.phone = phone;
    }

    @prop()
    username: string;
    @prop()
    passwordHash: string;
    @prop()
    roles: string[];
    @prop()
    phone: string;
}
