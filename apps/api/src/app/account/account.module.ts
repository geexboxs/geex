import { Module } from '@nestjs/common';
import { AccountResolver } from './account.resolver';
import { getModelForClass } from '@typegoose/typegoose';
import { User } from './models/user.model';
import { PasswordHasher } from './utils/password-hasher';
import { environment } from '@env';

@Module({
    imports: [],
    providers: [AccountResolver,
        {
            provide: PasswordHasher,
            useFactory: (injector) => new PasswordHasher(environment.authConfig.tokenSecret),
        },
    ],
})
export class AccountModule { }
