import { Inject } from "@graphql-modules/di";
import { Request } from "apollo-env";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import { Headers } from "apollo-server-env";
import { GraphQLExtension } from "apollo-server-express";
import { DocumentNode } from "graphql";
import stringify = require("json-stringify-safe");
import { GeexContext } from "../utils/abstractions";
import { GeexLogger } from "../utils/logger";

export class GlobalLoggingExtension extends GraphQLExtension<GeexContext> {
    public _logger: GeexLogger;
    /**
     *
     */
    constructor(@Inject(GeexLogger) logger: GeexLogger) {
        super();
        this._logger = logger;
    }
    public requestDidStart({ context }: { context: GeexContext }) {
        if (context.session.req.body === undefined) {
            return;
        }
        const logContent = { headers: context.session.req.headers, requestBody: context.session.req.body };
        this._logger.info(stringify(logContent));
    }

    public willSendResponse({ graphqlResponse, context }: {
        graphqlResponse: GraphQLResponse;
        context: GeexContext;
    }) {
        this._logger.debug(stringify({ headers: context.session.req.headers, responseBody: graphqlResponse }));
    }
}
