import { Injectable, ExecutionContext, mixin, UnauthorizedException } from "@nestjs/common";
import { AuthGuard as NestAuthGuard } from "@nestjs/passport";
import { GqlExecutionContext } from "@nestjs/graphql";
import { SessionStore } from "../../authentication/models/session.model";
import { AppPermission } from "../permissions.const";
import { AccessControl } from "@geexbox/accesscontrol";
import { ServiceLocator } from '@geex/api-shared';

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

      let userAcl = ServiceLocator.instance.get(AccessControl).can(user.userId);
      return this.scopes.any(scope => userAcl.do(scope))
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
