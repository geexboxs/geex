import { GeexContext } from "../abstractions";
import { GeexLogger } from "../logger";
import { ResolverData, NextFn, MiddlewareInterface } from "type-graphql";
import { stringify } from "json5";
import { Inject, Injectable } from "@graphql-modules/di";
import { UserFriendlyError } from "./user-friendly-error";

@Injectable()
export class GlobalErrorHandlingMiddleware implements MiddlewareInterface<GeexContext> {
    /**
     *
     */
    constructor(@Inject(GeexLogger) private logger: GeexLogger) {
    }

    async use({ context, info }: ResolverData<GeexContext>, next: NextFn) {
        try {
            return await next();
        } catch (err) {
            if (err instanceof UserFriendlyError) {
                throw err;
            }
            // write error to file log
            this.logger.error(err, context, info);
            // hide errors from db like printing sql query
            if (process.env.NODE_ENV === "production") {
                throw new UserFriendlyError(err.message);
            }
            // rethrow the error
            throw err;
        }
    }
}
