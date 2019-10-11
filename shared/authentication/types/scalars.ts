import { GraphQLScalarType } from 'graphql';
import { GraphQLError } from 'graphql/error';
import { Kind } from 'graphql/language';
export const AuthTokenScalar = new GraphQLScalarType({
    name: 'AuthToken',
    description: 'string token',
    serialize(value) {
        if (typeof value !== 'string') {
            throw new TypeError(`Value is not string: ${value}`);
        }
        return value;
    },
    parseValue(value) {
        if (typeof value !== 'string') {
            throw new TypeError(`Value is not string: ${value}`);
        }
        return value;
    },
    parseLiteral(ast) {
        if (ast.kind !== Kind.STRING) {
            throw new GraphQLError(`Can only validate strings as token but got a: ${ast.kind}`);
        }
    },
});
//# sourceMappingURL=PhoneNumber.js.map
