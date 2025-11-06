import { Component, input, Self, signal } from '@angular/core';
import { BaseFormFieldValueAccessor } from '../base-form-field-value-accessor';
import { NgControl, ReactiveFormsModule } from '@angular/forms';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ValidationErrorDirective } from '../../../directives/validation-error.directive';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-password-form-field',
  imports: [
    ValidationErrorDirective,
    ReactiveFormsModule,
    MatFormField,
    MatInputModule,
    MatIcon
  ],
  templateUrl: './password-form-field.component.html',
  styleUrl: './password-form-field.component.scss'
})
export class PasswordFormFieldComponent extends BaseFormFieldValueAccessor {
  label = input<string>('');
  placeholder = input<string>('');
  class = input<string>('w-full mb-3');
  inputClass = input<string>('');
  hide = signal(true);

  get type(): string {
    return this.hide() ? 'password' : 'text';
  }

  get icon(): string {
    return this.hide() ? 'visibility_off' : 'visibility';
  }

  constructor(@Self() controlDir: NgControl) {
    super(controlDir);
  }

  clickEvent(event: MouseEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.hide.set(!this.hide());
  }
}
