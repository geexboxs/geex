import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MessagingMessagesComponent } from './messages/messages.component';
import { MessagingViewComponent } from './messages/view/view.component';
import { MessagingEditComponent } from './messages/edit/edit.component';

const routes: Routes = [
  { path: '', component: MessagingMessagesComponent },
  { path: 'view/:id', component: MessagingViewComponent },
  { path: 'edit/:id', component: MessagingEditComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MessagingRoutingModule {}
