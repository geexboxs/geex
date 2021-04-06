import { Injectable, Injector } from '@angular/core';

import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild } from '@angular/router';
import { ACLService } from '@delon/acl';
import { JWTGuard, JWTTokenModel, ITokenService } from '@delon/auth';

@Injectable()
export class AuthGuard extends JWTGuard {
  constructor(private _aclService: ACLService, private _tokenService: ITokenService, private _router: Router, injector: Injector) {
    super(_tokenService, injector);
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (!this._tokenService.get()) {
      this._router.navigate(['/passport/login']);
      return false;
    }

    if (!route.data || !route.data['permission']) {
      return true;
    }

    if (this._aclService.can(route.data['permission'])) {
      return true;
    }

    this._router.navigate(['/exception/403']);
    return false;
  }

  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.canActivate(route, state);
  }
}
