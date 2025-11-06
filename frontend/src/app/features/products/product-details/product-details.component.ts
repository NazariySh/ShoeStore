import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Shoe } from '../../../shared/models/shoes/shoe';
import { ActivatedRoute } from '@angular/router';
import { ShoeService } from '../../../core/services/shoes/shoe.service';
import { CartService } from '../../../core/services/carts/cart.service';
import { CommonModule } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatDivider } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';

@Component({
  selector: 'app-product-details',
  imports: [
    CommonModule,
    FormsModule,
    MatDivider,
    MatButton,
    MatIcon,
    MatFormFieldModule,
    MatInput
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  product?: Shoe;
  id!: string;
  quantityInCart = 0;
  quantity = 1;

  @ViewChild('carousel', { static: true }) carousel!: ElementRef<HTMLDivElement>;

  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly shoeService: ShoeService,
    private readonly cartService: CartService
  ) {}

  get buttonText(): string {
    return this.quantityInCart > 0 ? 'Update cart' : 'Add to cart';
  }

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.getProduct(id);
    }
  }

  scrollCarousel(direction: number): void {
    const carousel = this.carousel.nativeElement;
    const scrollAmount = carousel.offsetWidth;
    carousel.scrollBy({ left: direction * scrollAmount, behavior: 'smooth' });
  }

  updateCart(): void {
    if (!this.product) return;

    if (this.quantity > this.quantityInCart) {
      const itemsToAdd = this.quantity - this.quantityInCart;
      this.cartService.addItemToCart(this.product, itemsToAdd).subscribe(
        () => {
          this.quantityInCart += itemsToAdd;
        }
      );
    }
    else {
      const itemsToRemove = this.quantityInCart - this.quantity;
      this.cartService.removeItemFromCart(this.product.shoeId, itemsToRemove).subscribe(
        () => {
          this.quantityInCart -= itemsToRemove;
        }
      );
    }
  }

  private getProduct(id: string): void {
    this.shoeService.getById(id).subscribe(
      product => {
        this.product = product;
        this.id = product.shoeId;
        this.updateQuantity();
      }
    );
  }

  private updateQuantity(): void {
    if (!this.product) return;

    this.quantityInCart = this.cartService.productQuantityInCart(this.product.shoeId);
    this.quantity = this.quantityInCart;
  }
}
