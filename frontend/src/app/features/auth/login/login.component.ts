import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth/auth.service';
import { BaseFormComponent } from '../../../shared/components/base/base-form-component';
import { AccountService } from '../../../core/services/accounts/account.service';
import { MatCard } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { FormFieldComponent } from '../../../shared/components/form-fields/form-field/form-field.component';
import { PasswordFormFieldComponent } from '../../../shared/components/form-fields/password-form-field/password-form-field.component';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { Login } from '../../../shared/models/auth/login';

@Component({
  selector: 'app-login',
  imports: [
    FormFieldComponent,
    PasswordFormFieldComponent,
    ReactiveFormsModule,
    MatCard,
    MatButton
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent extends BaseFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  readonly form = this.fb.group({ 
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });
  private returnUrl = '/shop';

  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly router: Router,
    private readonly authService: AuthService,
    private readonly accountService: AccountService,
    private readonly snackbar: SnackbarService
  ) {
    super();
  }

  ngOnInit(): void {
    const returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'];
    if (returnUrl) {
      this.returnUrl = returnUrl;
    }
  }

  onSubmit(): void {
    const login = this.mapFormToLogin();
    if (!login) {
      return;
    }

    this.authService.login(login).subscribe({
      next: () => {
        this.accountService.getProfile().subscribe(
          () => this.router.navigateByUrl(this.returnUrl)
        )
      },
      error: this.handleFormErrors,
    })
  }

  private mapFormToLogin(): Login | null {
    const { email, password } = this.form.value;
    if (!email || !password) {
      return null;
    }

    return { email, password };
  }
}
