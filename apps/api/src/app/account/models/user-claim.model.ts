import { prop } from "@typegoose/typegoose";
import { Field } from "@nestjs/graphql";
import { PhoneNumberResolver, EmailAddressResolver } from "graphql-scalars";
import { User } from "./user.model";
import { ModelBase } from '@geex/api-shared';



export class UserClaims extends ModelBase {
    @prop()
    @Field((type) => PhoneNumberResolver)
    public phone?: string;
    @prop()
    @Field((type) => EmailAddressResolver)
    public email?: string;
    @prop()
    @Field()
    public avatarUrl: string = "";
    @prop({ required: true, ref: "User" })
    @Field()
    public userId!: string;
    @Field()
    public nickname!: string;
}
