import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { User } from './account/models/user.model';
import { getModelForClass } from '@typegoose/typegoose';
import { Role } from './user-manage/model/role.model';
import { UserRole } from './user-manage/model/user-role.model';
import { appConfig } from '../configs/app-config';
import { Permission } from './authorization/models/permission.model';

const REEXPORTS = [
    MongooseModule.forRoot(appConfig.connections.mongo, { useNewUrlParser: true, useUnifiedTopology: true }),
    MongooseModule.forFeature([
        { name: nameof(User), schema: getModelForClass(User).schema },
        { name: nameof(Role), schema: getModelForClass(Role).schema },
        { name: nameof(Permission), schema: getModelForClass(Permission).schema },
        { name: nameof(UserRole), schema: getModelForClass(UserRole).schema },
    ])];

@Module({
    imports: [...REEXPORTS],
    providers: [],
    exports: [...REEXPORTS]
})
export class SharedModule { };
