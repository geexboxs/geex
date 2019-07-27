import { GraphQLModule } from '@graphql-modules/core';
import { UserModule } from './user/user.module';
import { buildSchemaSync } from 'type-graphql';
import { UserResolver } from './user/user.resolver';
import { LogInfo } from './custom-middlewares/middlewares/LogInfo';
import { SharedModule } from '../shared/shared.module';

export const AppModule = new GraphQLModule({
    imports: [UserModule, SharedModule],
    
    // extraSchemas: [
    //     buildSchemaSync({
    //         globalMiddlewares: [LogInfo],
    //         resolvers,
    //     })
    // ]
});
