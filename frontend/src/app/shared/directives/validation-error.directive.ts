import { Directive, ElementRef, inject, Input, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { distinctUntilChanged, merge, Subscription, tap } from 'rxjs';
import { ValidationService } from '../../core/services/validation.service';

@Directive({
  selector: '[appValidationError]'
})
export class ValidationErrorDirective implements OnInit, OnDestroy {
  private subscription: Subscription = Subscription.EMPTY;

  @Input({
    alias: 'appValidationError',
    required: true,
  })
  control: AbstractControl | null = null;

  constructor(
    private readonly elementRef: ElementRef,
    private readonly validationService: ValidationService,
  ) {}

  ngOnInit(): void {
    this.subscribeToControlChanges();
    this.updateErrorMessage();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  private subscribeToControlChanges(): void {
    if (!this.control) {
      return;
    }

    this.subscription = merge(
      this.control.statusChanges,
      this.control.valueChanges
    )
    .pipe(
        distinctUntilChanged(),
        tap(() => this.updateErrorMessage())
    ).subscribe();
  }

  private updateErrorMessage() {
    const errorMessage = this.validationService.getErrorMessage(this.control?.errors);
    this.elementRef.nativeElement.textContent = errorMessage || '';
  }
}
