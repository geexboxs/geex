import { RequiredPartial } from "../../../types";
import { ObjectType, Field } from "@nestjs/graphql";

@ObjectType()
export class Permission {
    @Field()
    name: string;
    children: Permission[] = [];
    parent?: Permission;
    constructor(init: RequiredPartial<Permission, "name">) {
        Object.assign(this, init);
        this.name = init.name;
    }
}
