import { Field, InputType, ArgsType } from 'type-graphql';
import { CreateUserInput } from './CreateUserInput';

@ArgsType()
export class StudentInput extends CreateUserInput {
    @Field()
    universityName?: string;
}
