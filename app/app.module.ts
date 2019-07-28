import { GraphQLModule } from '@graphql-modules/core';
import { UserModule } from './user/user.module';
import { buildSchemaSync } from 'type-graphql';
// import { UserResolver } from './user/user.resolver';
import { LogInfo } from './custom-middlewares/middlewares/LogInfo';
import { GeexLogger } from '../shared/logging/Logger';

export const AppModule = new GraphQLModule({
    imports: [UserModule],
    providers: [{
        provide: GeexLogger, useValue: new GeexLogger({})
    },],
    // extraSchemas: [
    //     buildSchemaSync({
    //         globalMiddlewares: [LogInfo],
    //         resolvers,
    //     })
    // ]
});
