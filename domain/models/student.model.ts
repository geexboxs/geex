import { ObjectType, Field, Authorized } from 'type-graphql';
import { User } from './user.model';
import { StudentInput } from '../../utils/CreateStudentInput';
import { prop } from 'typegoose';

@ObjectType()

export class Student extends User {
    /**
     *
     */
    constructor() {
        super();
    }
    @Field()
    @prop()
    universityName?: string;
    async Init(input: StudentInput) {
        await super.Init(input);
        this.universityName = input.universityName;
    }
}

export const StudentModel = new Student().getModelForClass(Student, {
    schemaOptions: {
        collection:"users"
    }
});
export const StudentModelToken = Symbol("StudentModel");
