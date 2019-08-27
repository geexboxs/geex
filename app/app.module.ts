import { GraphQLModule } from '@graphql-modules/core';
import { UserModule } from './user/user.module';
import { buildSchemaSync } from 'type-graphql';

export const AppModule = new GraphQLModule({
    imports: [UserModule],
});
