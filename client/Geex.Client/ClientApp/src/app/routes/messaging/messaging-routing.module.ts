import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MessagingMessagesComponent } from './messages/messages.component';

const routes: Routes = [{ path: 'messages', component: MessagingMessagesComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MessagingRoutingModule {}
