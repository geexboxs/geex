import { defaultFieldResolver, DirectiveLocation, GraphQLField, GraphQLFieldResolver, GraphQLInterfaceType, GraphQLObjectType, GraphQLSchema } from "graphql";
import { VisitableSchemaType } from "graphql-tools/dist/schemaVisitor";
import { MiddlewareInterface, NextFn, ResolverData, UnauthorizedError } from "type-graphql";
import { GeexContext } from "../utils/abstractions";
import { GeexLogger } from "../utils/logger";
import { GeexRoles } from "./roles";

/**
 * Define the schema directive
 * Should somehow define arguments and their types
 */
export class AuthMiddleware implements MiddlewareInterface<GeexContext> {
    constructor(private readonly logger: GeexLogger) { }

    public async use({ context, info }: ResolverData<GeexContext>, next: NextFn) {
        const user = context.user;
        const roles = user && user.roles;
        if (!user) {
            throw new UnauthorizedError();

        }
        if (roles === undefined) {
            return user !== undefined;
        }

        if (user.roles.any() && user.roles.intersect(roles).any()) {
            return next();
        }
        throw new UnauthorizedError();
    }
}
