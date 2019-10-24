import { MaxLength, Length, Min, Max } from "class-validator";

import { Int, InputType, Field, ArgsType } from "type-graphql";
import { User } from "./user.model";

@InputType()
export class RegisterInput implements Partial<User> {
    @Field()
    @MaxLength(30)
    username!: string;

    @Field()
    @Length(30, 255)
    password!: string;
}
