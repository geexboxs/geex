import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { Apollo, gql } from 'apollo-angular';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styles: [],
})
export class TestComponent implements OnInit {
  value = '111';
  constructor(private apollo: Apollo) {}

  ngOnInit(): void {
    this.apollo
      .watchQuery({
        query: gql`
          query {
            queryUsers {
              id
            }
          }
        `,
      })
      .valueChanges.subscribe((result: any) => {
        this.value = JSON.stringify(result?.data);
        let loading = result.loading;
        let error = result.error;
      });
  }
}
