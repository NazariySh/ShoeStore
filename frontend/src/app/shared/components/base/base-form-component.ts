import { FormGroup } from "@angular/forms";
import { ValidationService } from "../../../core/services/validation.service";
import { ValidationErrorResponse } from "../../models/validation-error-response";
import { inject } from "@angular/core";

export abstract class BaseFormComponent {
  readonly abstract form: FormGroup;
  private readonly validationService = inject(ValidationService);

  protected handleFormErrors = (error: ValidationErrorResponse) => {
    this.validationService.handleErrors(error, this.form);
  }
}