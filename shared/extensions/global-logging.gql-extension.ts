import { Inject } from "@graphql-modules/di";
import { Request } from "apollo-env";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import { Headers } from "apollo-server-env";
import { GraphQLExtension } from "apollo-server-express";
import { DocumentNode } from "graphql";
import stringify = require("json-stringify-safe");
import { IGeexContext } from "../utils/abstractions";
import { GeexLogger } from "../utils/logger";

export class GlobalLoggingExtension extends GraphQLExtension<IGeexContext> {
    public logger: GeexLogger;
    /**
     *
     */
    constructor(@Inject(GeexLogger) logger: GeexLogger) {
        super();
        this.logger = logger;
    }
    public requestDidStart({ context }: { context: IGeexContext }) {
        if (context.session.req.body === undefined) {
            return;
        }
        const logContent = { headers: context.session.req.headers, requestBody: context.session.req.body };
        this.logger.info(stringify(logContent));
    }

    public willSendResponse({ graphqlResponse, context }: {
        graphqlResponse: GraphQLResponse;
        context: IGeexContext;
    }) {
        this.logger.debug(stringify({ headers: context.session.req.headers, responseBody: graphqlResponse }));
    }
}
