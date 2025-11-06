import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CartService } from '../../core/services/carts/cart.service';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state.component';
import { CartItem } from '../../shared/models/carts/cart-item';
import { CartItemComponent } from './cart-item/cart-item.component';
import { OrderSummaryComponent } from '../orders/order-summary/order-summary.component';

@Component({
  selector: 'app-cart',
  imports: [
    OrderSummaryComponent,
    CartItemComponent,
    EmptyStateComponent
  ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent {

  get isEmpty(): boolean {
    return this.cartService.isEmpty();
  }

  get items(): CartItem[] {
    return this.cartService.cart()?.items ?? [];
  }

  constructor(
    private readonly cartService: CartService,
    private readonly router: Router
  ) {}

  navigateHome(): void {
    this.router.navigateByUrl('/shop');
  }
}
