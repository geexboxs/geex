
import { Module } from '@nestjs/common';
import { GraphQLModule } from '@nestjs/graphql';
import { AccountModule } from './account/account.module';
import { join } from 'lodash';
import { appConfig } from '../configs/app-config';
import { AuthenticationModule } from './authentication/authentication.module';
import { UserManageModule } from './user-manage/user-manage.module';
import { AuthorizationModule } from './authorization/authorization.module';
import { SharedModule } from './shared.module';
import { ServiceLocator } from '../shared/utils/service-locator';

@Module({
    imports: [
        SharedModule,
        AccountModule,
        AuthenticationModule,
        AuthorizationModule,
        UserManageModule,
        GraphQLModule.forRoot({
            installSubscriptionHandlers: true,
            autoSchemaFile: 'schema.gql',
            context: (args) => ({ ...args, injector: ServiceLocator.instance }),
        }),
    ],
})
export class AppModule { }
