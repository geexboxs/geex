import { GeexContext } from "./utils/abstractions";
import { GeexLogger } from "./utils/logger";
import { ResolverData, NextFn, MiddlewareInterface } from "type-graphql";
import { stringify } from "json5";
import { Inject, Injectable } from "@graphql-modules/di";

@Injectable()
export class GlobalLoggingMiddleware implements MiddlewareInterface<GeexContext> {
    /**
     *
     */
    constructor(@Inject(GeexLogger) private logger: GeexLogger) {
    }

    async use({ context, info }: ResolverData<GeexContext>, next: NextFn) {
        try {
            if (info.parentType.isTypeOf === undefined && (info.parentType.name === "Query" || info.parentType.name === "Mutation" || info.parentType.name === "Subscription")) {
                let logContent = { headers: context.session.req.headers, query: context.session.req.body }
                this.logger.debug(stringify(logContent));
            }
        } catch (error) {
            console.error(error)
        }
        return next();
    }
}
