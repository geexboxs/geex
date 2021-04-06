import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@shared';
import { DashboardComponent } from './dashboard.component';

const COMPONENTS = [DashboardComponent];

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild([
      {
        path: '',
        component: DashboardComponent,
      },
    ]),
  ],
  exports: [RouterModule],
  declarations: [...COMPONENTS],
})
export class DashboardModule {}
