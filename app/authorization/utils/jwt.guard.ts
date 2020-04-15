import { Injectable, ExecutionContext } from "@nestjs/common";
import { AuthGuard as NestAuthGuard } from "@nestjs/passport";
import { GqlExecutionContext } from "@nestjs/graphql";

@Injectable()
export class AuthGuard extends NestAuthGuard("jwt") {
    getRequest(context: ExecutionContext) {
        const ctx = GqlExecutionContext.create(context).getContext<ExecutionContext>();
        return ctx.req;
    }
}
