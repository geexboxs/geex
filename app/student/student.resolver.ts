import { Resolver, Query, Mutation, Authorized, Arg, FieldResolver, Root, Args, ResolverInterface } from "type-graphql";
import { Student } from "../../domain/models//student.model";
import { StudentInput} from '../../utils/CreateStudentInput';
import { InstanceType, ModelType } from "typegoose";
import { ClientSession } from "mongoose";
@Resolver(of => Student)
export class StudentResolver {
    constructor(
        private studentModel: ModelType<Student>
    ) { }
    
    @Mutation(returns => Student)
    async addStudent(@Args() StudentInput: StudentInput): Promise<Student> {
        let curSession!: ClientSession;
        await this.studentModel.db.startSession(undefined, async (err, session) => {
            curSession = session;
            session.startTransaction();
        });
        let student = new Student();
        await student.Init(StudentInput)
        let result = await this.studentModel.create(student);
        await curSession.commitTransaction();
        return result;
    }
}
