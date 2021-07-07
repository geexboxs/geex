import { NgModule, Type } from '@angular/core';
import { SharedModule } from '@shared';
import { MessagingRoutingModule } from './messaging-routing.module';
import { MessagingMessagesComponent } from './messages/messages.component';
import { MessagingMessagesEditComponent } from './messages/edit/edit.component';
import { MessagingMessagesViewComponent } from './messages/view/view.component';

const COMPONENTS: Type<void>[] = [MessagingMessagesComponent, MessagingMessagesEditComponent, MessagingMessagesViewComponent];

@NgModule({
  imports: [SharedModule, MessagingRoutingModule],
  declarations: COMPONENTS,
})
export class MessagingModule {}
