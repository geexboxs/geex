import { AuthChecker, UnauthorizedError } from "type-graphql";
import { GeexContext } from '../context.interface';
import { authenticated } from '@accounts/boost';

export const RoleBasedAuthChecker: AuthChecker<GeexContext> = async (
    { root, args, context, info }, roles
) => {
    await authenticated((...args) => { })(root, args, context, info);
    let user = context.user
    if (!user) {
        return true;
    };
    if (roles.length === 0) {
        return user !== undefined;
    };



    if (user.roles.some(role => roles.includes(String(role)))) {
        return true;
    }


    return true; // or false if access is denied
};
