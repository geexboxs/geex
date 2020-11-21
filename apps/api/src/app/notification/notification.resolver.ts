import { Inject } from '@nestjs/common';
import { REQUEST } from '@nestjs/core';
import { Mutation, Args, Resolver } from '@nestjs/graphql';
import { InjectModel } from '@nestjs/mongoose';
import { DocumentType } from '@typegoose/typegoose';
import { ModelType } from '@typegoose/typegoose/lib/types';
import { ObjectID } from 'bson';
import { ExecutionContext } from 'graphql/execution/execute';
import { SessionStore } from '../authentication/models/session.model';
import { Notification } from './models/notification.model';

@Resolver((of) => Notification)
export class NotificationResolver {
  constructor(
    @InjectModel(Notification.name)
    private notificationModel: ModelType<Notification>,
    @Inject(SessionStore)
    private sessionStore: SessionStore,
    @Inject(REQUEST)
    private request: ExecutionContext,
  ) { }

  @Mutation(() => Boolean)
  public async sendNotification(@Args({ name: "identifiers", type: () => [String] }) identifiers: string[], @Args({ name: "roles", type: () => [String] }) roles: string[]) {

  }
}
