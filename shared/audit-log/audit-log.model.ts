import { prop } from "@typegoose/typegoose";
import { Field, ObjectType } from "type-graphql";

@ObjectType()
export class AuditLog {
    /**
     *
     */
    constructor(content: string) {
        this.content = content;
    }
    @Field()
    content: string
}
