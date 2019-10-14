import { prop } from "@typegoose/typegoose";

export class User {

    @prop()
    public username: string;
    @prop()
    public passwordHash: string;
    @prop()
    public roles: string[];
    @prop()
    public phone: string;
    /**
     *
     */
    constructor({ username, passwordHash, phone, roles }: Readonly<User>) {
        this.username = username;
        this.passwordHash = passwordHash;
        this.roles = roles;
        this.phone = phone;
    }
}
