import { Directive, effect, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../../core/services/auth/auth.service';

@Directive({
  selector: '[appIsAdmin]'
})
export class IsAdminDirective {
  constructor(
    private readonly authService: AuthService,
    private readonly viewContainerRef: ViewContainerRef,
    private readonly templateRef: TemplateRef<any>
  ) {
    effect(() => {
      if (this.authService.isAdmin()) {
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      } else {
        this.viewContainerRef.clear();
      }
    })
  }
}
