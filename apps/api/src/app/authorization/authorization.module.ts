import { Module } from '@nestjs/common';
import { getModelForClass } from '@typegoose/typegoose';
import { AppConfig } from '@geex/api/app/app_config';
import { JwtModule } from '@nestjs/jwt';
import { User } from '../account/models/user.model';
import { PasswordHasher } from '../account/utils/password-hasher';
import * as ioredis  from "ioredis";
import { AuthorizationResolver } from './authorization.resolver';
import { UserPermissionChangeHandler } from './handlers/user-permission-change.handler';
const Handlers = [UserPermissionChangeHandler];
@Module({
    imports: [
    ],
    providers: [AuthorizationResolver,
        {
            provide: ioredis,
            useValue: new ioredis(AppConfig.connections.redis),
        },
        {
            provide: PasswordHasher,
            useFactory: () => new PasswordHasher(AppConfig.authConfig.tokenSecret),
        },
        ...Handlers
    ],
})
export class AuthorizationModule { }
