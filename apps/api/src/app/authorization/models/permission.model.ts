import { ObjectType, Field, ResolveField } from "@nestjs/graphql";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { prop, plugin } from "@typegoose/typegoose";
import { AppPermission, APP_PERMISSIONS } from "../permissions.const";
import { PermissionScalar } from "../scalars/permission.scalar";
import { RequiredPartial } from '@geex/api-shared';




@ObjectType()
export class PermissionNode {
    @Field(_ => PermissionScalar)
    @prop({
        unique: true,
    })
    name: string;



    constructor(init: RequiredPartial<PermissionNode, "name">) {
        Object.assign(this, init);
        this.name = init.name;
    }

    get childPermissions() {
        return Object.values(APP_PERMISSIONS).where(x => (x && new RegExp(`^${this.name}\..+`).test(x)) == true).map(x => new PermissionNode({ name: x }));
    }

    get parent() {
        let segments = this.name.split(".");
        segments.pop();
        let name = Object.values(APP_PERMISSIONS).find(x => x == segments.join("."));

        return name ? new PermissionNode({ name }) : undefined;
    };
}
