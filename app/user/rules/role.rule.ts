import { rule } from "graphql-shield";
import { IGeexContext } from "../../../types";
import { Enforcer } from "casbin";

export const role = (roles: string[]) =>
    rule()(
        async (parent, args, ctx: Required<IGeexContext>, info) => {
            return ctx.session.getUser().roles.intersect(roles).any();
        },
    );
