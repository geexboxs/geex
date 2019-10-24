import { ObjectType, Field } from "type-graphql";
import { ModelBase } from "../../../shared/utils/model-base";
import { prop } from "@typegoose/typegoose";
import { DateTime } from "graphql-scalars/dist/esnext/resolvers";

@ObjectType()
export class Session extends ModelBase {
    @prop()
    @Field(() => String)
    public refreshToken!: string;
    @prop()
    @Field(() => String)
    public accessToken!: string;
    @prop()
    @Field(() => DateTime)
    public expireAt!: Date;
}
