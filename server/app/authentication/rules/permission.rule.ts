import { rule } from "graphql-shield";
import { Enforcer } from "casbin";
import { ExecutionContext } from "@nestjs/common";
export const permission = (permissionName: string) =>
    rule()(
        async (parent, args, ctx: Required<ExecutionContext>, info) => {
            const enforcer = ctx.injector.get(Enforcer);
            return enforcer.hasPermissionForUser(ctx.getUser().userId, permissionName);
        },
    );
