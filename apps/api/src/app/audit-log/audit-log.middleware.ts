import { MiddlewareInterface, NextFn, ResolverData } from "type-graphql";
import json5 = require("json5");
import { ExecutionContext, Injectable, Inject } from "@nestjs/common";
import { GeexLogger } from '@geex/api-shared';

@Injectable()
export class AuditLogMiddleware implements MiddlewareInterface<ExecutionContext> {
    /**
     *
     */
    constructor(@Inject(GeexLogger) private logger: GeexLogger) {
    }

    public async use({ context, info }: ResolverData<ExecutionContext>, next: NextFn) {
        try {
            if (info.parentType.isTypeOf === undefined && (info.parentType.name === "Query" || info.parentType.name === "Mutation" || info.parentType.name === "Subscription")) {
                const logContent = { headers: context.req.headers, query: context.req.body };
                this.logger.debug(json5.stringify(logContent));
            }
        } catch (error) {
            console.error(error);
        }
        return next();
    }
}
