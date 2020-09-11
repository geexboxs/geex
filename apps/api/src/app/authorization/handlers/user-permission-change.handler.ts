import { CommandHandler, ICommandHandler } from "@nestjs/cqrs";
import { UserPermissionChangeCommand } from "../commands/change-user-role.command";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { InjectModel } from "@nestjs/mongoose";
import { User } from "../../account/models/user.model";
import { SessionStore } from "../../authentication/models/session.model";
import { Inject } from "@nestjs/common";
import { Role } from "../../user-manage/model/role.model";
@CommandHandler(UserPermissionChangeCommand)
export class UserPermissionChangeHandler implements ICommandHandler<UserPermissionChangeCommand> {
    constructor(
        @Inject(SessionStore)
        private sessionStore: SessionStore,
        @InjectModel(User.name)
        private userModel: ModelType<User>,
    ) { }

    async execute(command: UserPermissionChangeCommand) {
        // let usersToRefresh: User[] = [];
        // if (command.userOrRole instanceof User) {
        //     const user = await this.userModel.findById(command.userOrRole.id).exec();
        //     user && usersToRefresh.push(user);
        // }
        // if (command.userOrRole instanceof Role) {

        //     let role = await this.roleModel.findById(command.userOrRole.id).exec();
        //     role?.userRoles && (usersToRefresh = role?.userRoles.map(x => x.user).filter(x => x != undefined) as User[]);
        // }
        // await Promise.all(usersToRefresh.map(async user => {
        //     const userContext = await user?.toContextUser();
        //     userContext && await this.sessionStore.createOrRefresh(userContext);
        // }));
    }
}
