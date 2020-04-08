import { Module } from '@nestjs/common';
import { UserModelToken } from '../../shared/tokens';
import { getModelForClass } from '@typegoose/typegoose';
import { appConfig } from '../../configs/app-config';
import { EmailSender } from '../../shared/utils/email-sender';
import { MongooseModule } from '@nestjs/mongoose';
import { SessionResolver } from './authentication.resolver';
import { PassportModule } from '@nestjs/passport';
import { JwtModule } from '@nestjs/jwt';
import { AccessControl } from "@geexbox/accesscontrol";
import { User } from '../account/models/user.model';
import { PasswordHasher } from '../account/utils/password-hasher';
import ioredis = require("ioredis");

@Module({
    imports: [
        MongooseModule.forFeature([{ name: nameof(User), schema: getModelForClass(User).schema }]),
        PassportModule,
        JwtModule.register({
            secret: appConfig.authConfig.tokenSecret,
            signOptions: { expiresIn: appConfig.authConfig.expiresIn },
        })],
    providers: [SessionResolver,
        {
            provide: ioredis,
            useValue: new ioredis(appConfig.connections.redis),
        },
        AccessControl,
        {
            provide: PasswordHasher,
            useFactory: (injector) => new PasswordHasher(appConfig.authConfig.tokenSecret),
        },
    ],
})
export class SessionModule { }
