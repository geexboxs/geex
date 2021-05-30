import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SettingsService, User } from '@delon/theme';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styles: [],
})
export class AccountComponent implements OnInit {
  constructor(private router: Router, private cdr: ChangeDetectorRef, private settings: SettingsService) {
    this.user = settings.user;
  }
  private router$!: Subscription;
  user: User;

  ngOnInit(): void {}

  ngOnDestroy(): void {
    this.router$.unsubscribe();
  }
}
