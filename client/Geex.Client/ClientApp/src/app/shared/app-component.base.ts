import { Injector } from '@angular/core';
import { Apollo } from 'apollo-angular';

export abstract class AppComponentBase {
  apollo: Apollo;
  constructor(injector: Injector) {
    this.apollo = injector.get(Apollo);
  }
}
