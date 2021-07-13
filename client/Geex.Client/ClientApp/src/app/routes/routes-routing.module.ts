import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JWTGuard } from '@delon/auth';
import { environment } from '@env/environment';
// layout
import { LayoutBlankComponent } from '../layout/blank/blank.component';
import { LayoutBasicComponent } from '../layout/basic/basic.component';
// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutBasicComponent,
    canActivate: [JWTGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
      },
      // 业务子模块
      { path: 'account', loadChildren: () => import('./account/account.module').then((m) => m.AccountModule) },
      { path: 'messaging', loadChildren: () => import('./messaging/messaging.module').then((m) => m.MessagingModule) },
    ],
  },
  // 空白布局
  {
    path: 'blank',
    component: LayoutBlankComponent,
    children: [],
  },
  // passport
  {
    path: 'passport',
    loadChildren: () => import('./passport/passport.module').then((m) => m.PassportModule),
  },
  { path: 'exception', loadChildren: () => import('./exception/exception.module').then((m) => m.ExceptionModule) },
  { path: 'account', loadChildren: () => import('./account/account.module').then((m) => m.AccountModule) },
  { path: '**', redirectTo: 'exception/404' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      useHash: environment.useHash,
      enableTracing: !environment.production,
      // NOTICE: If you use `reuse-tab` component and turn on keepingScroll you can set to `disabled`
      // Pls refer to https://ng-alain.com/components/reuse-tab
      // scrollPositionRestoration: 'top',
    }),
  ],
  exports: [RouterModule],
})
export class RouteRoutingModule {}
