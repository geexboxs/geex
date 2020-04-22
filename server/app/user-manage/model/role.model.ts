// import { Address } from './address.model';
import { prop, DocumentType, plugin, pre, Ref } from "@typegoose/typegoose";
import { PhoneNumberResolver, EmailAddressResolver } from "graphql-scalars";
import { ObjectId } from "mongodb";
import { Document, Model, Schema, Types } from "mongoose";
import { ModelBase } from "../../../shared/utils/model-base";
import { Injector } from "@graphql-modules/di";
import { ObjectType, Field } from "@nestjs/graphql";
import { NestContainer, ModulesContainer } from "@nestjs/core";
import { User } from "../../account/models/user.model";
import { ServiceLocator } from "../../../shared/utils/service-locator";
import { CommandBus } from "@nestjs/cqrs";
import { UserPermissionChangeCommand } from "../../authorization/commands/change-user-role.command";
import { RefType } from "@typegoose/typegoose/lib/types";
import { AccessControl } from "@geexbox/accesscontrol";




@ObjectType()
export class Role {
    @Field()
    public name!: string;

    /**
     *
     */
    constructor(name: string) {
        this.name = name;
    }
    async setRolePermissions(permissions: string[]) {
        let acc = ServiceLocator.instance.get(AccessControl);
        permissions.forEach(x => acc.grant(this.name).do(x));
        await ServiceLocator.instance.get(CommandBus).execute(new UserPermissionChangeCommand(this));
    }
    public async getOwnedUsers() {
        let acc = ServiceLocator.instance.get(AccessControl);
        let userIds = acc.getInheritedRolesOf(this.name);
        let users = await ServiceLocator.instance.getModel(User).find({ _id: { $in: userIds } }).exec();
        return users;
    }

    public async getPermissions() {
        let acc = ServiceLocator.instance.get(AccessControl);
        return acc.getPermissionsOf(this.name);
    }
}
