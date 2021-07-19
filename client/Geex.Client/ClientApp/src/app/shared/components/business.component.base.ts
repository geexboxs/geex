import { ChangeDetectorRef, Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, DefaultUrlSerializer, NavigationEnd, Router } from '@angular/router';
import { ModalHelper } from '@delon/theme';
import { Apollo } from 'apollo-angular';
import { asyncScheduler, combineLatest, Observable, of, scheduled, Subscription } from 'rxjs';
import { NzMessageService } from 'ng-zorro-antd/message';
import { concatAll, defaultIfEmpty, filter, map, startWith } from 'rxjs/operators';
import { ACLService } from '@delon/acl';
import { ACLCanType } from '@delon/acl';
import { LoadingService } from '../services/loading.service';
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
  lifetimeSub: Subscription;
  acl: ACLService;
  loadingSrv: LoadingService;
  loading: boolean;
  abstract $init: Observable<any>;

  constructor(injector: Injector) {
    this.apollo = injector.get(Apollo);
    this.modal = injector.get(ModalHelper);
    this.route = injector.get(ActivatedRoute);
    this.router = injector.get(Router);
    this.cdr = injector.get(ChangeDetectorRef);
    this.msgSrv = injector.get(NzMessageService);
    this.acl = injector.get(ACLService);
    this.loadingSrv = injector.get(LoadingService);
    // this.$routeChange = this.router.events.pipe(
    //   // identify navigation end
    //   filter((event) => event instanceof NavigationEnd),
    //   // now query the activated route
    //   map(() => this.rootRoute(this.route)),
    //   filter((route: ActivatedRoute) => route.outlet === 'primary'),
    //   map((route: ActivatedRoute) => {
    //     console.error("trigger");
    //     return { ...route.snapshot.params, ...route.snapshot.queryParams, _fragment: route.snapshot.fragment }
    //   })
    // );
    // this.router.events.pipe(filter(x => x instanceof NavigationEnd), map((x: NavigationEnd) => {
    //   let route = new DefaultUrlSerializer().parse(x.url);
    //   return [route.]
    // }))
    this.$routeChange = combineLatest([
      this.route.params,
      this.route.queryParams,
      this.route.fragment,
      scheduled([of(null), this.router.events.pipe(filter((event) => event instanceof NavigationEnd))], asyncScheduler).pipe(concatAll()),
    ]).pipe<any>(
      map((x) => {
        console.error(x);
        return { ...x[0], ...x[1], _fragment: x[2] };
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
    this.lifetimeSub.add(this.loadingSrv.$loading.subscribe((x) => (this.loading = x)));
  }
  isGranted(permission: ACLCanType) {
    return true;
    return this.acl.can(permission);
  }

  private rootRoute(route: ActivatedRoute): ActivatedRoute {
    while (route.firstChild) {
      route = route.firstChild;
    }
    return route;
  }
}
