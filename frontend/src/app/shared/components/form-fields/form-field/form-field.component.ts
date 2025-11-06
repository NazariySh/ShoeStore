import { Component, input, Input, Self } from '@angular/core';
import { ValidationErrorDirective } from '../../../directives/validation-error.directive';
import { NgControl, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BaseFormFieldValueAccessor } from '../base-form-field-value-accessor';

@Component({
  selector: 'app-form-field',
  imports: [
    ValidationErrorDirective,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './form-field.component.html',
  styleUrl: './form-field.component.scss'
})
export class FormFieldComponent extends BaseFormFieldValueAccessor {
  label = input<string>('');
  type = input<string>('text');
  placeholder = input<string>('');
  class = input<string>('w-full mb-3');
  inputClass = input<string>('');
  min = input<number | null>(null);
  max = input<number | null>(null);
  step = input<number | null>(null);

  constructor(@Self() controlDir: NgControl) {
    super(controlDir);
  }
}
