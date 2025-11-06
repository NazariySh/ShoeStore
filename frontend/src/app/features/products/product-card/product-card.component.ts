import { Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { RouterLink } from '@angular/router';
import { Shoe } from '../../../shared/models/shoes/shoe';
import { CurrencyPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { CartService } from '../../../core/services/carts/cart.service';
import { SnackbarService } from '../../../core/services/snackbar.service';

@Component({
  selector: 'app-product-card',
  imports: [
    CurrencyPipe,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIcon
  ],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.scss'
})
export class ProductCardComponent {
  product = input.required<Shoe>();

  constructor(
    private readonly cartService: CartService,
    private readonly snackbar: SnackbarService
  ) {}

  get mainImage(): string {
    const product = this.product();
    const mainExistingImage = product.images.find(img => img.isMain);
    if (mainExistingImage) {
      return mainExistingImage.url;
    } else if (product.images.length > 0) {
      return product.images[0].url;
    } else {
      return 'images/no-image.png';
    }
  }

  addToCart(event: Event): void {
    event.stopPropagation();
    event.preventDefault();
    this.cartService.addItemToCart(this.product()).subscribe(
      () => {
        this.snackbar.success('Product added to cart');
      }
    );;
  }
}
