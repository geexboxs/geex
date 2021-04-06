import { Component, Injector, OnDestroy } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { _HttpClient } from '@delon/theme';
import { NzSafeAny } from 'ng-zorro-antd/core/types';
import { NzMessageService } from 'ng-zorro-antd/message';
import { AppComponentBase } from '../../../shared/app-component.base';
import { CaptchaProvider, GenerateCaptchaGqlMutation } from '../../../shared/graphql/.generated';
@Component({
  selector: 'passport-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.less'],
})
export class UserRegisterComponent extends AppComponentBase implements OnDestroy {
  constructor(
    injector: Injector,
    fb: FormBuilder,
    private router: Router,
    public http: _HttpClient,
    public msg: NzMessageService,
    private generateCaptchaGqlMutation: GenerateCaptchaGqlMutation,
  ) {
    super(injector);
    this.form = fb.group({
      mail: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required, Validators.minLength(6), UserRegisterComponent.checkPassword.bind(this)]],
      confirm: [null, [Validators.required, Validators.minLength(6), UserRegisterComponent.passwordEquar]],
      mobilePrefix: ['+86'],
      mobile: [null, [Validators.required, Validators.pattern(/^1\d{10}$/)]],
      captcha: [null, [Validators.required]],
    });
  }

  // #region fields

  get mail(): AbstractControl {
    return this.form.controls.mail;
  }
  get password(): AbstractControl {
    return this.form.controls.password;
  }
  get confirm(): AbstractControl {
    return this.form.controls.confirm;
  }
  get mobile(): AbstractControl {
    return this.form.controls.mobile;
  }
  get captcha(): AbstractControl {
    return this.form.controls.captcha;
  }
  form: FormGroup;
  error = '';
  type = 0;
  visible = false;
  status = 'pool';
  progress = 0;
  passwordProgressMap: { [key: string]: 'success' | 'normal' | 'exception' } = {
    ok: 'success',
    pass: 'normal',
    pool: 'exception',
  };

  // #endregion

  // #region get captcha

  count = 0;
  interval$: any;

  static checkPassword(control: FormControl): NzSafeAny {
    if (!control) {
      return null;
    }
    const self: any = this;
    self.visible = !!control.value;
    if (control.value && control.value.length > 9) {
      self.status = 'ok';
    } else if (control.value && control.value.length > 5) {
      self.status = 'pass';
    } else {
      self.status = 'pool';
    }

    if (self.visible) {
      self.progress = control.value.length * 10 > 100 ? 100 : control.value.length * 10;
    }
  }

  static passwordEquar(control: FormControl): { equar: boolean } | null {
    if (!control || !control.parent!) {
      return null;
    }
    if (control.value !== control.parent!.get('password')!.value) {
      return { equar: true };
    }
    return null;
  }

  async getCaptcha(): Promise<void> {
    if (this.mobile.invalid) {
      this.mobile.markAsDirty({ onlySelf: true });
      this.mobile.updateValueAndValidity({ onlySelf: true });
      return;
    }
    this.count = 59;
    await this.generateCaptchaGqlMutation
      .mutate({ captchaProvider: CaptchaProvider.Sms, smsCaptchaPhoneNumber: this.mobile.value })
      .toPromise();
    this.interval$ = setInterval(() => {
      this.count -= 1;
      if (this.count <= 0) {
        clearInterval(this.interval$);
      }
    }, 1000);
  }

  // #endregion

  submit(): void {
    this.error = '';
    Object.keys(this.form.controls).forEach((key) => {
      this.form.controls[key].markAsDirty();
      this.form.controls[key].updateValueAndValidity();
    });
    if (this.form.invalid) {
      return;
    }

    const data = this.form.value;
    this.http.post('/register?_allow_anonymous=true', data).subscribe(() => {
      this.router.navigate(['passport', 'register-result'], { queryParams: { email: data.mail } });
    });
  }

  ngOnDestroy(): void {
    if (this.interval$) {
      clearInterval(this.interval$);
    }
  }
}
