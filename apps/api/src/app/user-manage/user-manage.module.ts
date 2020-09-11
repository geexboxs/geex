import { Module } from '@nestjs/common';
import { User } from '../account/models/user.model';
import { getModelForClass } from '@typegoose/typegoose';
import { UserManageResolver } from './user-manage.resolver';
import { Role } from './model/role.model';

@Module({
    imports: [],
    providers: [UserManageResolver,
    ],
})
export class UserManageModule { }
