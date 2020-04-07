import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { User } from '../account/models/user.model';
import { getModelForClass } from '@typegoose/typegoose';
import { UserManageResolver } from './user-manage.resolver';
import { Role } from './model/role.model';
import { UserRole } from './model/user-role.model';

@Module({
    imports: [MongooseModule.forFeature([
        { name: nameof(User), schema: getModelForClass(User).schema },
        { name: nameof(Role), schema: getModelForClass(Role).schema },
        { name: nameof(UserRole), schema: getModelForClass(UserRole).schema },
    ])],
    providers: [UserManageResolver,
    ],
})
export class UserManageModule { }
