import { AuthChecker } from "type-graphql";
import { GeexContext } from "../context.interface";

export const RoleChecker: AuthChecker<GeexContext> = async (
    { root, args, context, info }, roles
) => {
    if (!context.user) {
        return false;
    }
    let allowRoles: string[] = [];
    if (allowRoles) {

    }


    return true; // or false if access is denied
};
