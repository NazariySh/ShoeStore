import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Register } from '../../../shared/models/auth/register';
import { BaseFormComponent } from '../../../shared/components/base/base-form-component';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { FormFieldComponent } from '../../../shared/components/form-fields/form-field/form-field.component';
import { PasswordFormFieldComponent } from '../../../shared/components/form-fields/password-form-field/password-form-field.component';
import { AuthService } from '../../../core/services/auth/auth.service';

@Component({
  selector: 'app-register',
  imports: [
    FormFieldComponent,
    PasswordFormFieldComponent,
    ReactiveFormsModule,
    MatCard,
    MatButton
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent extends BaseFormComponent {
  private readonly fb = inject(FormBuilder);
  readonly form = this.fb.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: ['', [Validators.required]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  constructor(
    private readonly router: Router,
    private readonly authService: AuthService,
    private readonly snackbar: SnackbarService
  ) {
    super();
  }

  onSubmit(): void {
    const register = this.mapFormToRegister();
    if (!register) {
      return;
    }

    this.authService.register(register).subscribe({
      next: () => {
        this.snackbar.success('Registration successful - you can now login');
        this.router.navigateByUrl('/auth/login');
      },
      error: this.handleFormErrors
    })
  }

  private mapFormToRegister(): Register | null {
    const { firstName, lastName, email, phoneNumber, password } = this.form.value;
    if (!firstName || !lastName || !email || !phoneNumber || !password) {
      return null;
    }

    return {
      firstName,
      lastName,
      email,
      phoneNumber,
      password
    };
  }
}
