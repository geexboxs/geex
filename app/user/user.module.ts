import { GraphQLModule } from '@graphql-modules/core';
// import { UserResolver } from './user.resolver';
import { buildSchemaSync, Resolver, Query, ObjectType, Field } from 'type-graphql';
import { DomainModule } from '../../domain/domain.module';
import { RoleBasedAuthChecker } from "../../shared/authentication/role-based-auth-checker";
// import { StudentModelToken, StudentModel } from '../../domain/models/student.model';
@ObjectType()
export class Test {
    @Field()
    name!: string;
}

@Resolver(of => Test)
export class TestResolver {
    @Query(type => Test)
    tests() {
        let hehe = new Test();
        hehe.name = "123"
        return hehe;
    }
}
const resolvers = [TestResolver];
export const UserModule: GraphQLModule = new GraphQLModule({
    extraSchemas: [
        buildSchemaSync({
            authChecker: RoleBasedAuthChecker,
            resolvers: [...resolvers],
            container: ({ ...args }) => {
                UserModule.injector.addChild(DomainModule.injector);
                return UserModule.injector.getSessionInjector(args.context);
            },
            // container: ({ context }) => context.injector

        })
    ],
    providers: [...resolvers],
    imports: [DomainModule]
});
