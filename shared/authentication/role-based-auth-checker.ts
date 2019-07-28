import { AuthChecker } from "type-graphql";
import { GeexContext } from '../context.interface';
import { authenticated } from '@accounts/boost';

export const RoleBasedAuthChecker: AuthChecker<GeexContext> = async (
    { root, args, context, info }, roles
) => {
    await authenticated(() => { })(root, args, context, info);
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
