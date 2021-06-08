import { Component, OnInit } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { I18N } from '../../core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit {
  I18N = I18N;
  constructor(private http: _HttpClient) {}

  ngOnInit(): void {}
}
