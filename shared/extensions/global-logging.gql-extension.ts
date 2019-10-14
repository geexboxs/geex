import { GraphQLExtension } from "apollo-server-express";
import { Request } from "apollo-env";
import { DocumentNode } from "graphql";
import { GraphQLRequestContext, GraphQLResponse } from "apollo-server-core";
import { Headers } from "apollo-server-env";
import stringify = require("json-stringify-safe");
import { GeexLogger } from "../utils/logger";
import { GeexContext } from "../utils/abstractions";
import { Inject } from "@graphql-modules/di";

export class GlobalLoggingExtension extends GraphQLExtension<GeexContext> {
    _logger: GeexLogger;
    /**
     *
     */
    constructor(@Inject(GeexLogger) logger: GeexLogger) {
        super();
        this._logger = logger;
    }
    requestDidStart({ context }: { context: GeexContext }) {
        if (context.session.req.body === undefined) {
            return;
        }
        let logContent = { headers: context.session.req.headers, requestBody: context.session.req.body }
        this._logger.info(stringify(logContent));
    }

    willSendResponse({ graphqlResponse, context }: {
        graphqlResponse: GraphQLResponse;
        context: GeexContext;
    }) {
        this._logger.debug(stringify({ headers: context.session.req.headers, responseBody: graphqlResponse }));
    }
}
