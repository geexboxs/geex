import { rule } from "graphql-shield";

export const permission = (roles: string[]) =>
    rule()(
        async (parent, args, ctx, info) => ctx,
    );
