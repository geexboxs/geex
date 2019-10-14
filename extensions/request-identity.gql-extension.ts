import { GraphQLExtension } from "apollo-server-express";
import { Request } from "apollo-env";
import { DocumentNode } from "graphql";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import rid = require('rid');

export class RequestIdentityExtensionClass<TContext = any> extends GraphQLExtension<TContext> {
    /**
     *
     */
    constructor() {
        super();
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

export const RequestIdentityExtension = () => new RequestIdentityExtensionClass();
