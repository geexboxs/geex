import { Module } from '@nestjs/common';
import { User } from '../account/models/user.model';
import { getModelForClass } from '@typegoose/typegoose';
import { UserManageResolver } from './user-manage.resolver';
import { Role } from './model/role.model';
import { UserRole } from './model/user-role.model';
import { SharedModule } from '../shared.module';

@Module({
    imports: [SharedModule],
    providers: [UserManageResolver,
    ],
})
export class UserManageModule { }
