import { AuthChecker, UnauthorizedError } from "type-graphql";
import { GeexContext } from '../context.interface';
import acl from "acl";
export const DefaultAuthChecker: AuthChecker<GeexContext> = async (
    { root, args, context, info }, roles
) => {
    let user = context.user
    if (!user) {
        return false;
    };
    if (roles.length === 0) {
        return user !== undefined;
    };

    if (user.roles.any() && user.roles.intersect(roles).any()) {
        return true;
    }

    return false; // or false if access is denied
};
