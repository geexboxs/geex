import { GraphQLExtension } from "apollo-server-express";
import { Request } from "apollo-env";
import { DocumentNode } from "graphql";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import { Headers } from "apollo-server-env";
import { GeexLogger } from "./Logger";
import { stringify } from "json5";
import rid = require('rid');

const { print } = require('graphql');

export class RequestIdentityExtension<TContext = any> extends GraphQLExtension<TContext> {
    _logger: GeexLogger;
    /**
     *
     */
    constructor(logger: GeexLogger) {
        super();
        this._logger = logger;
    }
    requestDidStart({ request,
        queryString,
        parsedQuery,
        operationName,
        variables,
        persistedQueryHit,
        persistedQueryRegister,
        context,
        requestContext }: {
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
        if (request.headers.has("X-RID")) {
            return;
        }
        request.headers.set("X-RID", rid())
    }
}
