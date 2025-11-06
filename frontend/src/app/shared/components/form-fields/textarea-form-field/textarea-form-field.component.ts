import { Component, input, Self } from '@angular/core';
import { BaseFormFieldValueAccessor } from '../base-form-field-value-accessor';
import { ReactiveFormsModule, NgControl } from '@angular/forms';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ValidationErrorDirective } from '../../../directives/validation-error.directive';

@Component({
  selector: 'app-textarea-form-field',
  imports: [
    ValidationErrorDirective,
    ReactiveFormsModule,
    MatFormField,
    MatInputModule
  ],
  templateUrl: './textarea-form-field.component.html',
  styleUrl: './textarea-form-field.component.scss'
})
export class TextareaFormFieldComponent extends BaseFormFieldValueAccessor {
  label = input<string>('');
  type = input<string>('text');
  placeholder = input<string>('');
  class = input<string>('w-full mb-3');
  inputClass = input<string>('');

  constructor(@Self() controlDir: NgControl) {
    super(controlDir);
  }
}
