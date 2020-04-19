import { Injectable, ExecutionContext, mixin, UnauthorizedException } from "@nestjs/common";
import { AuthGuard as NestAuthGuard } from "@nestjs/passport";
import { GqlExecutionContext } from "@nestjs/graphql";
import { SessionStore } from "../../authentication/models/session.model";
import { ServiceLocator } from "../../../shared/utils/service-locator";
import { AppPermission } from "../permissions.const";
import { AccessControl } from "@geexbox/accesscontrol";

export const AuthGuard = function (...scopes: AppPermission[]) {
    return mixin(class AuthGuard extends NestAuthGuard("jwt") {
        protected readonly scopes: string[] = [];
        /**
         *
         */
        constructor(
        ) {
            super();
            if (scopes?.any()) {
                this.scopes = scopes;
            }
        }

        async canActivate(context: ExecutionContext) {
            let result = await super.canActivate(context);
            let user = this.getRequest(context).user;
            if (!user) {
                throw new UnauthorizedException();
            }

            if (this.scopes?.any() == false) {
                return result && true;
            }

            let permissions = ServiceLocator.instance.get(AccessControl).getPermissionsOf(user.userId);

            if (permissions && this.scopes.intersect(permissions).any()) {
                return result && true;
            }

            throw new UnauthorizedException(`Required scopes (${this.scopes.join(',')})`);
        }
        getRequest(context: ExecutionContext) {
            const ctx = GqlExecutionContext.create(context).getContext<ExecutionContext>();
            return ctx.req;
        }


        protected get sessionStore() {
            return ServiceLocator.instance.get(SessionStore);
        }
    });
}
