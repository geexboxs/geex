import { AuthChecker } from "type-graphql";
import { GeexContext } from "../context.interface";

export const RoleChecker: AuthChecker<GeexContext> = async (
    { root, args, context, info }, roles
) => {
    if (!context.user) {
        return false;
    }
    let allowRoles: string[] = [];
    if (!allowRoles || !allowRoles.any()) {
        return true;
    }
    if (allowRoles.intersect(context.user.roles)) {
        return true;
    }
    return false; // or false if access is denied
};
