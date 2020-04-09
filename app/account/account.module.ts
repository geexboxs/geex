import { Module } from '@nestjs/common';
import { AccountResolver } from './account.resolver';
import { UserModelToken } from '../../shared/tokens';
import { getModelForClass } from '@typegoose/typegoose';
import { User } from './models/user.model';
import { PasswordHasher } from './utils/password-hasher';
import { appConfig } from '../../configs/app-config';
import { EmailSender } from '../../shared/utils/email-sender';
import { SharedModule } from '../shared.module';

@Module({
    imports: [SharedModule],
    providers: [AccountResolver,
        {
            provide: PasswordHasher,
            useFactory: (injector) => new PasswordHasher(appConfig.authConfig.tokenSecret),
        },
        {
            provide: EmailSender,
            useFactory: (injector) => appConfig.connections.smtp && new EmailSender(appConfig.connections.smtp.sendAs, {
                secure: appConfig.connections.smtp.secure,
                auth: {
                    user: appConfig.connections.smtp.username,
                    pass: appConfig.connections.smtp.password,
                },
                host: appConfig.connections.smtp.host,
                port: appConfig.connections.smtp.port,
            }),
        },
    ],
})
export class AccountModule { }
