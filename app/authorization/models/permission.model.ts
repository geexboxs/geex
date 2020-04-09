import { RequiredPartial } from "../../../types";
import { ObjectType, Field, ResolveField } from "@nestjs/graphql";
import { ServiceLocator } from "../../../shared/utils/service-locator";
import { ModelType } from "@typegoose/typegoose/lib/types";
import { ModelBase } from "../../../shared/utils/model-base";
import { prop } from "@typegoose/typegoose";

@ObjectType()
export class Permission extends ModelBase<Permission> {
    @Field()
    @prop({
        unique: true,
    })
    name: string;

    parent?: Permission;

    constructor(init: RequiredPartial<Permission, "name">) {
        super();
        Object.assign(this, init);
        this.name = init.name;
    }

    async childPermissions(): Promise<Permission[]> {
        return await ServiceLocator.instance.getModel(Permission).find({ name: { $regex: new RegExp(`^${this.name}\..+`) } }).exec();
    }
}
