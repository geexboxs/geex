import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { User } from './account/models/user.model';
import { getModelForClass } from '@typegoose/typegoose';
import { Role } from './user-manage/model/role.model';
import { appConfig } from '../configs/app-config';
import { PassportModule } from '@nestjs/passport';
import { JwtModule } from '@nestjs/jwt';
import { EmailSender } from '../shared/utils/email-sender';
import { UserClaims } from './account/models/user-claim.model';
import { PermissionScalar } from './authorization/scalars/permission.scalar';

const REEXPORTS = [
    PassportModule,
    JwtModule.register({
        secret: appConfig.authConfig.tokenSecret,
        signOptions: { expiresIn: appConfig.authConfig.expiresIn },
    }),
    MongooseModule.forRoot(appConfig.connections.mongo, {
        useNewUrlParser: true,
        useUnifiedTopology: true,
        connectionFactory: (connection) => {
            connection.plugin(require('mongoose-autopopulate'));
            return connection;
        },
    }),
    MongooseModule.forFeature([
        { name: nameof(User), schema: getModelForClass(User).schema },
        { name: nameof(Role), schema: getModelForClass(Role).schema },
        { name: nameof(UserClaims), schema: getModelForClass(UserClaims).schema },
    ])];

const PROVIDERS = [
    PermissionScalar,
]

@Module({
    imports: [...REEXPORTS],
    providers: [...PROVIDERS],
    exports: [...REEXPORTS],
})
export class SharedModule { }
