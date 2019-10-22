import { rule } from "graphql-shield";

export const role = (roles: string[]) =>
    rule()(
        async (parent, args, ctx, info) => ctx,
    );
