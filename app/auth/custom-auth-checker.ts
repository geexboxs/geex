import { AuthChecker } from "type-graphql";
import { Context } from './context.interface';

export const authChecker: AuthChecker<Context> = (
    { root, args, context, info }, roles
) => {
    let user = context["req"].user
    if (roles.length === 0) {
        return user !== undefined;
    };

    if (!user) {
        return false;
    };

    if (user.roles.some(role => roles.includes(String(role)))) {
        return true;
    }


    return true; // or false if access is denied
};
