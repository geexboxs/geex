import { Module } from '@nestjs/common';
import { UserModelToken } from '../../shared/tokens';
import { getModelForClass } from '@typegoose/typegoose';
import { appConfig } from '../../configs/app-config';
import { AuthenticationResolver } from './authentication.resolver';
import { JwtModule } from '@nestjs/jwt';
import { User } from '../account/models/user.model';
import { PasswordHasher } from '../account/utils/password-hasher';
import { SharedModule } from '../shared.module';
import { JwtStrategy } from './utils/jwt.stratage';

@Module({
    imports: [
        SharedModule,
        ],
    providers: [AuthenticationResolver,
        JwtStrategy,
        {
            provide: PasswordHasher,
            useFactory: (injector) => new PasswordHasher(appConfig.authConfig.tokenSecret),
        },
    ],
})
export class AuthenticationModule { }
