import { rule } from "graphql-shield";
import { IGeexContext } from "../../../types";
import { Enforcer } from "casbin";

export const role = (roles: string[]) =>
    rule()(
        async (parent, args, ctx: Required<IGeexContext>, info) => {
            const enforcer = ctx.injector.get(Enforcer);
            const userRoles = await enforcer.getRolesForUser(ctx.user.id);
            return userRoles.intersect(ctx.user.roles).any();
        },
    );
