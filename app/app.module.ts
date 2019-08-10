import { GraphQLModule } from '@graphql-modules/core';
import { UserModule } from './user/user.module';
import { buildSchemaSync } from 'type-graphql';
// import { UserResolver } from './user/user.resolver';
import { LogInfo } from './custom-middlewares/middlewares/LogInfo';

export const AppModule = new GraphQLModule({
    imports: [UserModule],
});
