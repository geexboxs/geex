import { Inject, Injectable } from "@graphql-modules/di";
import stringify = require("json-stringify-safe");
import { MiddlewareInterface, NextFn, ResolverData } from "type-graphql";
import { IGeexContext } from "../../types";
import { GeexLogger } from "../../shared/utils/logger";

@Injectable()
export class AuditLogMiddleware implements MiddlewareInterface<IGeexContext> {
    /**
     *
     */
    constructor(@Inject(GeexLogger) private logger: GeexLogger) {
    }

    public async use({ context, info }: ResolverData<IGeexContext>, next: NextFn) {
        try {
            if (info.parentType.isTypeOf === undefined && (info.parentType.name === "Query" || info.parentType.name === "Mutation" || info.parentType.name === "Subscription")) {
                const logContent = { headers: context.session.req.headers, query: context.session.req.body };
                this.logger.debug(stringify(logContent));
            }
        } catch (error) {
            console.error(error);
        }
        return next();
    }
}
