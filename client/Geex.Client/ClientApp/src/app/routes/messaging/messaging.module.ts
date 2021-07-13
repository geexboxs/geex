import { NgModule, Type } from '@angular/core';
import { SharedModule } from '@shared';
import { MessagingRoutingModule } from './messaging-routing.module';
import { MessagingMessagesComponent } from './messages/messages.component';
import { MessagingEditComponent } from './messages/edit/edit.component';
import { MessagingViewComponent } from './messages/view/view.component';

const COMPONENTS: Type<void>[] = [MessagingMessagesComponent, MessagingViewComponent, MessagingEditComponent];

@NgModule({
  imports: [SharedModule, MessagingRoutingModule],
  declarations: COMPONENTS,
})
export class MessagingModule {}
