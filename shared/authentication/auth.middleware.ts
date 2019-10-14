import { GraphQLField, GraphQLSchema, defaultFieldResolver, GraphQLFieldResolver, GraphQLInterfaceType, GraphQLObjectType, DirectiveLocation } from "graphql";
import { GeexContext } from "../utils/abstractions";
import { VisitableSchemaType } from "graphql-tools/dist/schemaVisitor";
import { UnauthorizedError, MiddlewareInterface, ResolverData, NextFn } from "type-graphql";
import { GeexRoles } from "./roles";
import { GeexLogger } from "../utils/logger";

/**
 * Define the schema directive
 * Should somehow define arguments and their types
 */
export class AuthMiddleware implements MiddlewareInterface<GeexContext> {
    constructor(private readonly logger: GeexLogger) { }

    async use({ context, info }: ResolverData<GeexContext>, next: NextFn) {
        const user = context.user;
        const roles = user && user.roles;
        if (!user) {
            throw UnauthorizedError;

        };
        if (roles === undefined) {
            return user !== undefined;
        };

        if (user.roles.any() && user.roles.intersect(roles).any()) {
            return next();
        }
        throw UnauthorizedError;
    }
}
