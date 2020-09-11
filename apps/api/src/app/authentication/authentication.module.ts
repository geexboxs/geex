import { Module } from '@nestjs/common';
import { getModelForClass } from '@typegoose/typegoose';
import { environment } from '@env';
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
            useFactory: (injector) => new PasswordHasher(environment.authConfig.tokenSecret),
        },
    ],
})
export class AuthenticationModule { }
