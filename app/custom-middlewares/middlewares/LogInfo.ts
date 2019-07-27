import { Service } from 'typedi';
import { MiddlewareInterface, ResolverData, NextFn, ArgumentValidationError } from 'type-graphql'

import { Context } from "../context";
import { GeexLogger } from '../../../shared/logging/Logger';

@Service()
export class LogInfo implements MiddlewareInterface<Context> {
    constructor(private readonly logger: GeexLogger) { }
    async use({ root, context, info, args }: ResolverData<Context>, next: NextFn) {
        
        try {
            const operation = info.operation.operation
            const url = context.req.headers.referer
            const body = context.req.body.query
            this.logger.debug(operation, url, body)
            return await next();
        } catch (error) {
            this.logger.error(error)
        }
    };

}

