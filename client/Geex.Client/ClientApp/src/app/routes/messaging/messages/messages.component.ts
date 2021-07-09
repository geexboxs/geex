import { Component, OnInit, ViewChild } from '@angular/core';
import { STColumn, STComponent } from '@delon/abc/st';
import { SFSchema } from '@delon/form';
import { ModalHelper, _HttpClient } from '@delon/theme';
import { Apollo } from 'apollo-angular';
import { MessageFragGql } from '../../../shared/graphql/.generated/type';

@Component({
  selector: 'app-messaging-messages',
  templateUrl: './messages.component.html',
})
export class MessagingMessagesComponent implements OnInit {
  data = this.apollo.query({
    query: MessageFragGql,
    variables: {
      userIdentifier: this.userName.value,
      password: this.password.value,
    },
  });
  searchSchema: SFSchema = {
    properties: {
      no: {
        type: 'string',
        title: '编号',
      },
    },
  };
  @ViewChild('st') private readonly st!: STComponent;
  columns: STColumn[] = [
    { title: '编号', index: 'no' },
    { title: '调用次数', type: 'number', index: 'callNo' },
    { title: '头像', type: 'img', width: '50px', index: 'avatar' },
    { title: '时间', type: 'date', index: 'updatedAt' },
    {
      title: '',
      buttons: [
        { text: '查看', click: (item: any) => `/form/${item.id}` },
        { text: '编辑', type: 'static', component: MessagingMessagesComponent, click: 'reload' },
      ],
    },
  ];

  constructor(private http: _HttpClient, public apollo: Apollo, private modal: ModalHelper) {}

  ngOnInit(): void {}

  add(): void {
    // this.modal
    //   .createStatic(FormEditComponent, { i: { id: 0 } })
    //   .subscribe(() => this.st.reload());
  }
}
