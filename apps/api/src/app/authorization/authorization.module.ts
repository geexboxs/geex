import { Module } from '@nestjs/common';
import { UserModelToken } from '../../shared/tokens';
import { getModelForClass } from '@typegoose/typegoose';
import { environments } from '../../configs/app-config';
import { JwtModule } from '@nestjs/jwt';
import { User } from '../account/models/user.model';
import { PasswordHasher } from '../account/utils/password-hasher';
import ioredis = require("ioredis");
import { SharedModule } from '../shared.module';
import { AuthorizationResolver } from './authorization.resolver';
import { UserPermissionChangeHandler } from './handlers/user-permission-change.handler';
const Handlers = [UserPermissionChangeHandler];
@Module({
    imports: [
        SharedModule,
    ],
    providers: [AuthorizationResolver,
        {
            provide: ioredis,
            useValue: new ioredis(environments.connections.redis),
        },
        {
            provide: PasswordHasher,
            useFactory: () => new PasswordHasher(environments.authConfig.tokenSecret),
        },
        ...Handlers
    ],
})
export class AuthorizationModule { }
