import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApolloQueryResult } from '@apollo/client/core';
import { STChange, STColumn, STComponent } from '@delon/abc/st';
import { SFComponent, SFSchema, SFValueChange } from '@delon/form';
import { ModalHelper, _HttpClient } from '@delon/theme';
import { Apollo } from 'apollo-angular';
import { Observable, pipe } from 'rxjs';
import { combineLatest } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';
import {
  MessageFragFragment,
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
export class MessagingMessagesComponent implements OnInit {
  $data: Observable<MessageFragFragment[]>;
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
  columns: STColumn<MessageFragFragment>[] = [
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
        { text: '查看', click: (item: any) => `/form/${item.id}` },
        { text: '编辑', type: 'static', component: MessagingMessagesComponent, click: 'reload' },
      ],
    },
  ];
  $param: Observable<any>;

  constructor(
    private http: _HttpClient,
    public apollo: Apollo,
    private modal: ModalHelper,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
  ) {
    this.$param = combineLatest([this.route.params, this.route.queryParams]).pipe<any>(
      map((x) => {
        return { ...x[0], ...x[1] };
      }),
    );
    let $res = this.$param.pipe(
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
          },
        });
      }),
    );

    this.$data = $res.pipe(
      map((x) => {
        console.log(x);
        this.st.total = x.data.messages.totalCount;
        return x.data.messages.items;
      }),
    );
  }

  async ngAfterViewInit(): Promise<void> {
    //Called after ngAfterContentInit when the component's view has been initialized. Applies to components only.
    //Add 'implements AfterViewInit' to the class.
  }
  stChange(args: STChange) {
    if (args.type == 'pi') {
      this.router.navigate([], { queryParams: { page: args.pi, title: this.sf.value.title } });
    }
  }

  ngOnInit(): void {}

  add(): void {
    // this.modal
    //   .createStatic(FormEditComponent, { i: { id: 0 } })
    //   .subscribe(() => this.st.reload());
  }
}
