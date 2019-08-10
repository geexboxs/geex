import { GraphQLExtension } from "apollo-server-express";
import { Request } from "apollo-env";
import { DocumentNode } from "graphql";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import { Headers } from "apollo-server-env";
import { GeexLogger } from "./Logger";
import { stringify } from "json5";

const { print } = require('graphql');

export class GlobalLoggingExtension<TContext = any> extends GraphQLExtension<TContext> {
    _logger: GeexLogger;
    /**
     *
     */
    constructor(logger: GeexLogger) {
        super();
        this._logger = logger;
    }
    requestDidStart(args: {
        request: Pick<Request, 'url' | 'method' | 'headers'>;
        queryString?: string;
        parsedQuery?: DocumentNode;
        operationName?: string;
        variables?: { [key: string]: any };
        persistedQueryHit?: boolean;
        persistedQueryRegister?: boolean;
        context: any;
        requestContext: GraphQLRequestContext<any>;
    }) {
        if (!args.queryString || args.queryString.startsWith("query IntrospectionQuery")) {
            return;
        }
        let logContent = { headers: Object.fromEntries(Array.from(args.request.headers)), query: args.queryString, variables: args.variables }
        this._logger.debug(stringify(logContent));
    }

    willSendResponse({ graphqlResponse, context }: {
        graphqlResponse: GraphQLResponse;
        context: TContext;
    }) {
        if ((graphqlResponse && graphqlResponse.data && graphqlResponse.data.__schema)) {
            return;
        }
        this._logger.debug(stringify(graphqlResponse));
    }
}
