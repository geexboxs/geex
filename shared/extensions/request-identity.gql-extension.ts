import { Request } from "apollo-env";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import { GraphQLExtension } from "apollo-server-express";
import { DocumentNode } from "graphql";
import rid = require("rid");
import { IGeexContext } from "../utils/abstractions";

export class RequestIdentityExtension extends GraphQLExtension<IGeexContext> {
    /**
     *
     */
    constructor() {
        super();
    }
    public requestDidStart({ context }: { context: IGeexContext }) {
        if (context.session.req.headers["X-RID"]) {
            return;
        }
        context.session.req.headers["X-RID"] = rid();
    }
}
