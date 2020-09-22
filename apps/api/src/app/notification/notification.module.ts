import { Module } from '@nestjs/common';
import { getModelForClass } from '@typegoose/typegoose';
import { environment } from '@env';
import { SendNotificationHandler } from './handlers/send-notification.handler';

@Module({
  imports: [],
  providers: [
    SendNotificationHandler
  ],
})
export class NotificationModule { }
