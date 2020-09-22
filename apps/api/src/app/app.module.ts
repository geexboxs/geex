
import { Module } from '@nestjs/common';
import { GraphQLModule } from '@nestjs/graphql';
import { AccountModule } from './account/account.module';
import { join } from 'lodash';
import { AuthenticationModule } from './authentication/authentication.module';
import { UserManageModule } from './user-manage/user-manage.module';
import { AuthorizationModule } from './authorization/authorization.module';
import { ServiceLocator, JaegerTraceExtension, ComplexityExtension } from '@geex/api-shared';
import { SharedModule } from './shared.module';
import { NotificationModule } from './notification/notification.module';


@Module({
  imports: [
    SharedModule,
    AccountModule,
    AuthenticationModule,
    AuthorizationModule,
    UserManageModule,
    NotificationModule,
    GraphQLModule.forRoot({
      installSubscriptionHandlers: true,
      autoSchemaFile: 'schema.gql',
      context: (args) => {
        return ({ ...args, injector: ServiceLocator.instance });
      },
      extensions: [
        () => ServiceLocator.instance.get(JaegerTraceExtension),
        () => ServiceLocator.instance.get(ComplexityExtension),
      ],
    }),
  ],
  providers: [JaegerTraceExtension, ComplexityExtension],
})
export class AppModule { }
