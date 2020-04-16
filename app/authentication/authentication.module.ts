import { Module } from '@nestjs/common';
import { UserModelToken } from '../../shared/tokens';
import { getModelForClass } from '@typegoose/typegoose';
import { appConfig } from '../../configs/app-config';
import { AuthenticationResolver } from './authentication.resolver';
import { JwtModule } from '@nestjs/jwt';
import { AccessControl } from "@geexbox/accesscontrol";
import { User } from '../account/models/user.model';
import { PasswordHasher } from '../account/utils/password-hasher';
import { SessionStore } from './models/session.model';
import ioredis = require("ioredis");
import { SharedModule } from '../shared.module';
import { JwtStrategy } from './utils/jwt.stratage';

@Module({
    imports: [
        SharedModule,
        ],
    providers: [AuthenticationResolver,
        SessionStore,
        {
            provide: ioredis,
            useValue: new ioredis(appConfig.connections.redis),
        },
        JwtStrategy,
        AccessControl,
        {
            provide: PasswordHasher,
            useFactory: (injector) => new PasswordHasher(appConfig.authConfig.tokenSecret),
        },
    ],
})
export class AuthenticationModule { }
