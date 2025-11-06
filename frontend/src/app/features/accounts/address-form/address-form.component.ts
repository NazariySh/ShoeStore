import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AccountService } from '../../../core/services/accounts/account.service';
import { BaseFormComponent } from '../../../shared/components/base/base-form-component';
import { AuthService } from '../../../core/services/auth/auth.service';
import { Router } from '@angular/router';
import { FormFieldComponent } from '../../../shared/components/form-fields/form-field/form-field.component';
import { MatButton } from '@angular/material/button';
import { Address } from '../../../shared/models/accounts/address';

@Component({
  selector: 'app-address-form',
  imports: [
    ReactiveFormsModule,
    FormFieldComponent,
    MatButton
  ],
  templateUrl: './address-form.component.html',
  styleUrl: './address-form.component.scss'
})
export class AddressFormComponent extends BaseFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  readonly form = this.fb.group({
    street: ['', [Validators.required]],
    city: ['', [Validators.required]],
    state: ['', [Validators.required]],
    country: ['', [Validators.required]],
    postalCode: ['', [Validators.required]],
  });

  constructor(
    private readonly router: Router,
    private readonly accountService: AccountService,
    private readonly authService: AuthService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.getAddress();
  }

  onSubmit(): void {
    const address = this.mapFormToAddress();
    if (!address) {
      return;
    }

    this.accountService.updateAddress(address).subscribe({
      next: () => {
        this.resetForm();
        this.router.navigateByUrl('/account/profile');
      },
      error: this.handleFormErrors
    });
  }

  onCancel(): void {
    this.resetForm();
    this.router.navigateByUrl('/account/profile');
  }

  private mapFormToAddress(): Address | null {
    const { street, city, state, country, postalCode } = this.form.value;
    if (!street || !city || !state || !country || !postalCode) {
      return null;
    }

    return {
      street,
      city,
      state,
      country,
      postalCode
    };
  }

  private getAddress(): void {
    const address = this.authService.currentUser()?.address;
    if (address) {
      this.form.patchValue(address);
    }
  }

  private resetForm(): void {
    this.form.reset();
  }
}
