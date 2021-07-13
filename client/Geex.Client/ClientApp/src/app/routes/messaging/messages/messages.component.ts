import { ChangeDetectorRef, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApolloQueryResult } from '@apollo/client/core';
import { STChange, STColumn, STComponent } from '@delon/abc/st';
import { SFComponent, SFSchema, SFValueChange } from '@delon/form';
import { ModalHelper, _HttpClient } from '@delon/theme';
import { Apollo } from 'apollo-angular';
import { Observable, pipe } from 'rxjs';
import { combineLatest } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';
import { BusinessComponentBase } from '../../../shared/components/business.component.base';
import {
  MessageBriefFragment,
  MessagesGql,
  MessagesQuery,
  MessagesQueryVariables,
  MessageSeverityType,
  CollectionSegmentInfo,
} from '../../../shared/graphql/.generated/type';

@Component({
  selector: 'app-messaging-messages',
  templateUrl: './messages.component.html',
})
export class MessagingMessagesComponent extends BusinessComponentBase {
  $init: Observable<any>;
  data: MessageBriefFragment[];
  searchSchema: SFSchema = {
    properties: {
      title: {
        type: 'string',
        title: '标题',
      },
    },
  };
  @ViewChild('sf')
  private readonly sf!: SFComponent;
  @ViewChild('st')
  private readonly st!: STComponent;
  columns: STColumn<MessageBriefFragment>[] = [
    { title: 'Id', index: 'id' },
    { title: '标题', index: 'title' },
    {
      title: '重要性',
      index: 'severity',
      type: 'badge',
      badge: {
        INFO: {
          text: '信息',
          color: 'default',
        },
      },
    },
    { title: '消息类型', index: 'messageType' },
    { title: '发送人', index: 'fromUserId' },
    { title: '发送时间', index: 'time', type: 'date' },
    // { title: '调用次数', type: 'number', index: 'callNo' },
    // { title: '头像', type: 'img', width: '50px', index: 'avatar' },
    // { title: '时间', type: 'date', index: 'updatedAt' },
    {
      title: '操作',
      buttons: [
        { text: '查看', click: (item: MessageBriefFragment) => this.router.navigate(['view', item.id], { relativeTo: this.route }) },
        { text: '编辑', click: (item: MessageBriefFragment) => this.router.navigate(['edit', item.id], { relativeTo: this.route }) },
      ],
    },
  ];

  constructor(injector: Injector) {
    super(injector);

    let $res = this.$routeChange.pipe(
      switchMap((param) => {
        return this.apollo.query<MessagesQuery, MessagesQueryVariables>({
          query: MessagesGql,
          variables: {
            skip: (param.page - 1) * (this.st?.ps ?? 10),
            take: this.st?.ps ?? 10,
            filter: {
              title: {
                contains: param.title ?? '',
              },
            },
            includeDetail: false,
          },
        });
      }),
    );

    this.$init = $res.pipe(
      map((x) => {
        this.st.total = x.data.messages.totalCount;
        this.loading = x.loading;
        this.data = x.data.messages.items;
      }),
    );
  }

  stChange(args: STChange) {
    if (args.type == 'pi') {
      this.router.navigate([], { queryParams: { page: args.pi, title: this.sf.value.title } });
    }
  }

  add(): void {
    // this.modal
    //   .createStatic(FormEditComponent, { i: { id: 0 } })
    //   .subscribe(() => this.st.reload());
  }
}
