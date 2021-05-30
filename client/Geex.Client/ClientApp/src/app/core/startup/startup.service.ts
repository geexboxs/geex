import { Injectable, Injector, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { zip } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { ALAIN_I18N_TOKEN, Menu, MenuService, SettingsService, TitleService } from '@delon/theme';
import { ACLService } from '@delon/acl';

import { NzIconService } from 'ng-zorro-antd/icon';
import { ICONS } from '../../../style-icons';
import { ICONS_AUTO } from '../../../style-icons-auto';
import { Apollo, gql } from 'apollo-angular';
import { InitSettingsGql, InitSettingsQuery } from 'src/app/shared/graphql/.generated/type';

/**
 * Used for application startup
 * Generally used to get the basic data of the application, like: Menu Data, User Data, etc.
 */
@Injectable()
export class StartupService {
  constructor(
    iconSrv: NzIconService,
    private menuService: MenuService,
    private settingService: SettingsService,
    private aclService: ACLService,
    private titleService: TitleService,
    @Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService,
    private httpClient: HttpClient,
    private injector: Injector,
    private apollo: Apollo,
  ) {
    iconSrv.addIcon(...ICONS_AUTO, ...ICONS);
  }

  private async viaHttp(): Promise<void> {
    let res = await this.apollo
      .query<InitSettingsQuery>({
        query: InitSettingsGql,
        variables: {},
      })
      .toPromise();
    const settings = res.data.settings;
    this.settingService.setApp({
      name: settings.first((x) => x.name == 'APP_NAME').value,
    });
    this.menuService.add(JSON.parse(settings.first((x) => x.name == 'APP_MENU').value) as Menu[]);
  }

  // private viaMock(resolve: any, reject: any): void {
  //   // const tokenData = this.tokenService.get();
  //   // if (!tokenData.token) {
  //   //   this.injector.get(Router).navigateByUrl('/passport/login');
  //   //   resolve({});
  //   //   return;
  //   // }
  //   // mock
  //   const app: any = {
  //     name: `ng-alain`,
  //     description: `Ng-zorro admin panel front-end framework`,
  //   };
  //   const user: any = {
  //     name: 'Admin',
  //     avatar: './assets/tmp/img/avatar.jpg',
  //     email: 'cipchk@qq.com',
  //     token: '123456789',
  //   };
  //   // Application information: including site name, description, year
  //   this.settingService.setApp(app);
  //   // User information: including name, avatar, email address
  //   this.settingService.setUser(user);
  //   // ACL: Set the permissions to full, https://ng-alain.com/acl/getting-started
  //   this.aclService.setFull(true);
  //   // Menu data, https://ng-alain.com/theme/menu
  //   this.menuService.add([
  //     {
  //       text: 'Main',
  //       group: true,
  //       children: [
  //         {
  //           text: 'Dashboard',
  //           link: '/dashboard',
  //           icon: { type: 'icon', value: 'appstore' },
  //         },
  //       ],
  //     },
  //   ]);
  //   // Can be set page suffix title, https://ng-alain.com/theme/title
  //   this.titleService.suffix = app.name;

  //   resolve({});
  // }

  load(): Promise<any> {
    // only works with promises
    // https://github.com/angular/angular/issues/15088
    return this.viaHttp();
  }
}
