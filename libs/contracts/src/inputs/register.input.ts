import { MaxLength, Length, Min, Max } from "class-validator";
import { InputType, Field } from "@nestjs/graphql";

@InputType()
export class RegisterInput {
    @Field()
    username!: string;

    @Field()
    @Length(6, 32)
    password!: string;
}
