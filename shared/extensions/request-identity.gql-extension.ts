import { GraphQLExtension } from "apollo-server-express";
import { Request } from "apollo-env";
import { DocumentNode } from "graphql";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import rid = require('rid');
import { GeexContext } from "../utils/abstractions";

export class RequestIdentityExtension extends GraphQLExtension<GeexContext> {
    /**
     *
     */
    constructor() {
        super();
    }
    requestDidStart({ context }: { context: GeexContext }) {
        if (context.session.req.headers["X-RID"]) {
            return;
        }
        context.session.req.headers["X-RID"] = rid()
    }
}
