import { ChangeDetectorRef, Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalHelper } from '@delon/theme';
import { Apollo } from 'apollo-angular';
import { combineLatest, Observable, Subscription } from 'rxjs';
import { NzMessageService } from 'ng-zorro-antd/message';
import { map } from 'rxjs/operators';
@Component({
  template: '',
})
export abstract class BusinessComponentBase implements OnInit, OnDestroy {
  apollo: Apollo;
  modal: ModalHelper;
  route: ActivatedRoute;
  router: Router;
  cdr: ChangeDetectorRef;
  $routeChange: Observable<any>;
  msgSrv: NzMessageService;
  loading: boolean;
  lifetimeSub: Subscription;
  abstract $init: Observable<any>;

  constructor(injector: Injector) {
    this.apollo = injector.get(Apollo);
    this.modal = injector.get(ModalHelper);
    this.route = injector.get(ActivatedRoute);
    this.router = injector.get(Router);
    this.cdr = injector.get(ChangeDetectorRef);
    this.msgSrv = injector.get(NzMessageService);

    this.$routeChange = combineLatest([this.route.params, this.route.queryParams, this.route.url]).pipe<any>(
      map((x) => {
        return { ...x[0], ...x[1] };
      }),
    );
  }
  ngOnDestroy(): void {
    this.lifetimeSub.unsubscribe();
  }
  ngOnInit(): void {
    if (this.$init == undefined) {
      throw new Error('please implement $init');
    }

    this.lifetimeSub = this.$init.subscribe();
  }
}
