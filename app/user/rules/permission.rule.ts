import { rule } from "graphql-shield";
import { IGeexContext } from "../../../types";
import { Enforcer } from "casbin";
export const permission = (permissionName: string) =>
    rule()(
        async (parent, args, ctx: Required<IGeexContext>, info) => {
            const enforcer = ctx.injector.get(Enforcer);
            return enforcer.hasPermissionForUser(ctx.session.getUser().id, permissionName);
        },
    );
