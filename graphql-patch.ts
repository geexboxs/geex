import { print } from 'graphql/language';
import graphql = require("graphql/utilities/schemaPrinter");
import { MethodAndPropDecorator } from 'type-graphql/dist/decorators/types';
import { MetadataStorage as MetadataStorage1 } from "type-graphql/dist/metadata/metadata-storage";

export class MetadataStorage extends MetadataStorage1 {
    private readonly schemaDirectives: Array<{ name: string, obj: object }> = [];
    private readonly resolvers: Array<{
        name: string,
        func: Function,
        type: object,
        directives: Array<{ name: string, args: object }>
    }> = [];

    addSchemaDirective(name: string, obj: object) {
        this.schemaDirectives.push({ name, obj });
    }

    getSchemaDirectives() {
        return this.schemaDirectives.reduce((acc, cur) => ({ ...acc, [cur.name]: cur.obj }), {});
    }

    addResolver(name: string, func: Function, type: object, directives: Array<{ name: string, args: object }>) {
        this.resolvers.push({ name, func, type, directives });
    }

    getResolvers() {
        return this.resolvers;
    }
}

let metadataStorage: MetadataStorage = global['TypeGraphQLMetadataStorage'] || (global['TypeGraphQLMetadataStorage'] = new MetadataStorage());

export const SchemaDirective = function (name: string): MethodAndPropDecorator {
    return (target) => {
        metadataStorage.addSchemaDirective(name, target);
    }
}

function printFields(options, type) {
    const fields = Object.values(type.getFields() as any[]).map(
        (f, i) =>
            graphql['printDescription'](options, f, '  ', !i) +
            '  ' +
            f.name +
            graphql['printArgs'](options, f.args, '  ') +
            ': ' +
            String(f.type) +
            // graphql['printDeprecated'](f),
            printFieldDirectives(f),
    );
    return graphql['printBlock'](fields);
}

function printFieldDirectives(field) {
    const directives = field.astNode.directives;
    return directives.map(d =>
        ' ' +
        '@' +
        d.name.value +
        printFieldDirectiveArgs(d.arguments)
    );
}

function printFieldDirectiveArgs(args) {
    const printArg = (arg: any) => arg.name.value + ': ' + print(arg.value);
    return args && args.length
        ? '(' + args.slice(1).reduce((acc, cur) => acc + ',' + printArg(cur), printArg(args[0])) + ')'
        : '';
}

graphql['printFields'] = printFields;
