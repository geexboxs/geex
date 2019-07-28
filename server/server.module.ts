import { GraphQLModule } from '@graphql-modules/core';

import { SharedModule } from '../shared/shared.module';
import { GeexLogger } from '../shared/logging/Logger';
import { AppModule } from '../app/app.module';

export const ServerModule = new GraphQLModule({
    imports: [AppModule, SharedModule],
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
