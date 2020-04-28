import { MaxLength, Length, Min, Max } from "class-validator";

import { User } from "../../../server/app/account/models/user.model";
import { InputType, Field } from "@nestjs/graphql";

@InputType()
export class RegisterInput implements Partial<User> {
    @Field()
    username!: string;

    @Field()
    @Length(6, 32)
    password!: string;
}
