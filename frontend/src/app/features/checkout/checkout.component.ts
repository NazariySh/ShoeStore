import { CommonModule, CurrencyPipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { RouterLink, Router } from '@angular/router';
import { AuthService } from '../../core/services/auth/auth.service';
import { CartService } from '../../core/services/carts/cart.service';
import { DeliveryMethodService } from '../../core/services/orders/delivery-method.service';
import { OrderService } from '../../core/services/orders/order.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { BaseFormComponent } from '../../shared/components/base/base-form-component';
import { SelectFormFieldComponent } from '../../shared/components/form-fields/select-form-field/select-form-field.component';
import { OrderCreate } from '../../shared/models/orders/order-create';
import { SelectItem } from '../../shared/models/select-item';

@Component({
  selector: 'app-checkout',
  imports: [
    SelectFormFieldComponent,
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButton,
    MatIcon,
    RouterLink
  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent extends BaseFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  readonly form = this.fb.group({
    deliveryMethodId: ['', [Validators.required]],
  });
  deliveryMethods: SelectItem<string>[] = [];

  constructor(
    private readonly orderService: OrderService,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly snackbar: SnackbarService,
    private readonly deliveryMethodService: DeliveryMethodService,
    private readonly cartService: CartService
  ) {
    super();
  }

  ngOnInit(): void {
    this.getDeliveryMethods();
  }

  get address() {
    return this.authService.currentUser()?.address;
  }

  get addressInfo(): string {
    const address = this.authService.currentUser()?.address;
    if (!address) {
      return 'No address found. Please update your profile.';
    }

    return `${address.street}, ${address.city}, ${address.state}, ${address.postalCode}, ${address.country}`;
  }

  onSubmit(): void {
    const orderCreate = this.mapFormToOrderCreate();
    if (!orderCreate) {
      return;
    }

    this.orderService.create(orderCreate).subscribe({
      next: () => {
        this.snackbar.success('Order created successfully!');
        this.router.navigate(['/orders']);
      },
      error: this.handleFormErrors
    })
  }

  private mapFormToOrderCreate(): OrderCreate | null {
    const cartId = this.cartService.cartId();
    if (!cartId) {
      this.snackbar.error('No cart found. Please add items to your cart.');
      this.router.navigate(['/shopping-cart']);
      return null;
    }

    if (!this.address) {
      this.snackbar.error('No address found. Please update your profile.');
      this.router.navigate(['/profile']);
      return null;
    }

    const { deliveryMethodId } = this.form.value;
    if (!deliveryMethodId) {
      return null;
    }

    return {
      deliveryMethodId,
      shoppingCartId: cartId
    };
  }

  private getDeliveryMethods() {
    this.deliveryMethodService.getAll().subscribe(
      (methods) => {
        this.deliveryMethods = methods.map((method) => ({
          value: method.deliveryMethodId,
          label: `${method.name} - ${new CurrencyPipe('en-US').transform(method.price)}`
        }));
      }
    );
  }
}
