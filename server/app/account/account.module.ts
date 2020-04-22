import { Module } from '@nestjs/common';
import { AccountResolver } from './account.resolver';
import { UserModelToken } from '../../shared/tokens';
import { getModelForClass } from '@typegoose/typegoose';
import { User } from './models/user.model';
import { PasswordHasher } from './utils/password-hasher';
import { appConfig } from '../../configs/app-config';
import { SharedModule } from '../shared.module';

@Module({
    imports: [SharedModule],
    providers: [AccountResolver,
        {
            provide: PasswordHasher,
            useFactory: (injector) => new PasswordHasher(appConfig.authConfig.tokenSecret),
        },
    ],
})
export class AccountModule { }
