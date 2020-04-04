import { Module } from '@nestjs/common';
import { AccountResolver } from './account.resolver';
import { UserModelToken } from '../../shared/tokens';
import { getModelForClass } from '@typegoose/typegoose';
import { User } from './models/user.model';
import { PasswordHasher } from './utils/password-hasher';
import { appConfig } from '../../configs/app-config';
import { EmailSender } from '../../shared/utils/email-sender';
import { MongooseModule } from '@nestjs/mongoose';

@Module({
    imports: [MongooseModule.forFeature([{ name: nameof(User), schema: getModelForClass(User).schema }])],
    providers: [AccountResolver,
        {
            provide: PasswordHasher,
            useFactory: (injector) => new PasswordHasher(appConfig.authConfig.tokenSecret),
        },
        {
            provide: EmailSender,
            useValue: appConfig.connections.smtp && new EmailSender(appConfig.connections.smtp.sendAs, {
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
