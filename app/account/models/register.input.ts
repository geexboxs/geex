import { MaxLength, Length, Min, Max } from "class-validator";

import { User } from "./user.model";
import { InputType, Field } from "@nestjs/graphql";

@InputType()
export class RegisterInput implements Partial<User> {
    @Field()
    username!: string;

    @Field()
    @Length(6, 32)
    password!: string;
}
