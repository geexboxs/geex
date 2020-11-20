import { Module } from '@nestjs/common';
import { getModelForClass } from '@typegoose/typegoose';
import { AppConfig } from '@geex/api/app/app_config';
import { AuthenticationResolver } from './authentication.resolver';
import { JwtModule } from '@nestjs/jwt';
import { User } from '../account/models/user.model';
import { PasswordHasher } from '../account/utils/password-hasher';
import { JwtStrategy } from './utils/jwt.stratage';

@Module({
    imports: [
        ],
    providers: [AuthenticationResolver,
        JwtStrategy,
        {
            provide: PasswordHasher,
            useFactory: (injector) => new PasswordHasher(AppConfig.authConfig.tokenSecret),
        },
    ],
})
export class AuthenticationModule { }
