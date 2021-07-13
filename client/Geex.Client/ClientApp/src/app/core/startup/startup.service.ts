import { Injectable, Injector, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { zip } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { AlainI18NService, ALAIN_I18N_TOKEN, Menu, MenuService, SettingsService, TitleService } from '@delon/theme';
import { ACLService } from '@delon/acl';

import { NzIconService } from 'ng-zorro-antd/icon';
import { ICONS } from '../../../style-icons';
import { ICONS_AUTO } from '../../../style-icons-auto';
import { Apollo, gql } from 'apollo-angular';
import { InitSettingsGql, InitSettingsQuery } from 'src/app/shared/graphql/.generated/type';
import { I18NService } from '../i18n/i18n.service';
import { ReuseTabMatchMode, ReuseTabService } from '@delon/abc/reuse-tab';

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
    @Inject(ALAIN_I18N_TOKEN) private i18n: I18NService,
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
      name: settings.first((x) => x.name == 'APP_APP_MENU').value,
    });
    this.menuService.add(JSON.parse(settings.first((x) => x.name == 'APP_APP_MENU').value) as Menu[]);
    this.i18n.merge(JSON.parse(settings.first((x) => x.name == 'LOCALIZATION_DATA').value));
  }

  load(): Promise<any> {
    // only works with promises
    // https://github.com/angular/angular/issues/15088
    return this.viaHttp();
  }
}
