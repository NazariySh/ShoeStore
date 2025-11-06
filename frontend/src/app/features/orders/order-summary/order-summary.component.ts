import { Component, inject } from '@angular/core';
import { CartService } from '../../../core/services/carts/cart.service';
import { CommonModule, Location } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-order-summary',
  imports: [
    CommonModule,
    MatButton,
    FormsModule,
    RouterLink
  ],
  templateUrl: './order-summary.component.html',
  styleUrl: './order-summary.component.scss'
})
export class OrderSummaryComponent {

  get subtotal(): number {
    return this.cartService.subtotal();
  }

  get shipping(): number {
    return this.cartService.shipping();
  }

  get totalAmount(): number {
    return this.cartService.totalAmount();
  }

  constructor(
    private readonly cartService: CartService
  ) {}
}
