import { Injectable } from '@angular/core';
import { ValidationErrorResponse } from '../../shared/models/validation-error-response';
import { FormGroup, ValidationErrors } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ValidationService {

  handleErrors(error: ValidationErrorResponse, form: FormGroup) {
    if (!error?.errors) {
      return;
    }

    Object.keys(error.errors).forEach((key) => {
      const formattedKey = key.charAt(0).toLowerCase() + key.slice(1); 
      const control = form.get(formattedKey);
      if (control) {
        control.setErrors({ serverError: error.errors[key] });
      }
    });
  }

  getErrorMessage(errors?: ValidationErrors | null) {
    if (!errors) return '';
    if (errors['required']) return 'This field is required.';
    if (errors['email']) return 'Invalid email format.';
    if (errors['minlength']) return `Minimum length is ${errors['minlength'].requiredLength} characters.`;
    if (errors['maxlength']) return `Maximum length is ${errors['maxlength'].requiredLength} characters.`;
    if (errors['pattern']) return 'Invalid format.';
    if (errors['serverError']) return errors['serverError'];
    return 'Invalid input.';
  }
}
