import { Scalar, CustomScalar } from '@nestjs/graphql';
import { Kind, ValueNode } from 'graphql';
import { AppPermission } from '../permissions.const';

@Scalar('PermissionScalar', type => PermissionScalar)
export class PermissionScalar implements CustomScalar<string, string> {
    description = 'PermissionScalar';

    parseValue(value: string): string {
        return value; // value from the client
    }

    serialize(value: string): string {
        return value; // value sent to the client
    }

    parseLiteral(ast: ValueNode): string {
        if (ast.kind === Kind.STRING) {
            return ast.value;
        }
        throw new TypeError("invalid permission name:" + ast);
    }
    
}
