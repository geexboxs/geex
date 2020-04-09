import { AuthChecker, UnauthorizedError } from "type-graphql";
import { IRule, allow } from "graphql-shield";
import { IShieldContext } from "graphql-shield/dist/types";
import objHash = require("object-hash");
import { ExecutionContext } from "@nestjs/common";
export const RbacAuthChecker: AuthChecker<ExecutionContext & IShieldContext, IRule> = (
    { root, args, context, info },
    funcs: IRule[],
) => {
    // here we can read the user from context
    // and check his permission in the db against the `roles` argument
    // that comes from the `@Authorized` decorator, eg. ["ADMIN", "MODERATOR"]
    return context.getUser() && funcs.every(async x => (await x.resolve(root, args, context, info, {
        fallbackError: new UnauthorizedError(),
        allowExternalErrors: true,
        debug: true,
        fallbackRule: allow,
        hashFunction: objHash,
    })) === true); // or false if access is denied
};
