import { IsOptional, Length, MaxLength } from 'class-validator';
import { Field, InputType, ArgsType } from 'type-graphql';
import { User } from '../domain/models/user.model';
@ArgsType()
export class CreateUserInput {
    @Field()
    @MaxLength(30)
    name!: string;

    @Field({ nullable: true })
    @Length(30, 255)
    // tslint:disable-next-line: variable-name
    surname!: string;

    @Field(returns=>[String])
    // tslint:disable-next-line: variable-name
    addresses?: string[];

}
