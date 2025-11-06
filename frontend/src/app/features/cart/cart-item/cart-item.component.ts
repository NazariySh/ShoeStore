import { Component, input } from '@angular/core';
import { CartItem } from '../../../shared/models/carts/cart-item';
import { CartService } from '../../../core/services/carts/cart.service';
import { RouterLink } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { CurrencyPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-cart-item',
  imports: [
    CurrencyPipe,
    RouterLink,
    MatIcon,
    MatButtonModule
  ],
  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.scss'
})
export class CartItemComponent {
  item = input.required<CartItem>();

  constructor(
    private readonly cartService: CartService
  ) {}

  incrementQuantity(): void {
    this.cartService.addItemToCart(this.item()).subscribe();
  }

  decrementQuantity(): void {
    this.cartService.removeItemFromCart(this.item().productId).subscribe();
  }

  removeItem(): void {
    this.cartService.removeItemFromCart(this.item().productId, this.item().quantity).subscribe();
  }
}
