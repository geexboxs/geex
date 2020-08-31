import { User } from "../../account/models/user.model";
import { Role } from "../../user-manage/model/role.model";

export class UserPermissionChangeCommand {
    constructor(
        public readonly userOrRole: User | Role,
    ) { }
}
