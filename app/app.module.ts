
import { Module } from '@nestjs/common';
import { GraphQLModule } from '@nestjs/graphql';
import { AccountModule } from './account/account.module';
import { join } from 'lodash';
import { appConfig } from '../configs/app-config';
import { MongooseModule } from '@nestjs/mongoose';

@Module({
    imports: [
        AccountModule,
        MongooseModule.forRoot(appConfig.connections.mongo, { useNewUrlParser: true, useUnifiedTopology: true }),
        GraphQLModule.forRoot({
            installSubscriptionHandlers: true,
            autoSchemaFile: 'schema.gql',
        }),
    ],
})
export class AppModule { }
