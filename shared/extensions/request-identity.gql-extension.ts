import { GraphQLExtension } from "apollo-server-express";
import { Request } from "apollo-env";
import { DocumentNode } from "graphql";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import rid = require('rid');
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";

export class RequestIdentityExtensionClass extends GraphQLExtension<ExpressContext> {
    /**
     *
     */
    constructor() {
        super();
    }
    requestDidStart({ ...args }) {
        if (args.context.session.req.headers["X-RID"]) {
            return;
        }
        args.context.session.req.headers["X-RID"] = rid()
    }
}

export const RequestIdentityExtension = () => new RequestIdentityExtensionClass();
