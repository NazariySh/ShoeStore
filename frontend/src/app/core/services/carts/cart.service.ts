import { computed, Injectable, signal } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ShoppingCart } from '../../../shared/models/carts/shopping-cart';
import { Observable, of, tap } from 'rxjs';
import { DeliveryMethod } from '../../../shared/models/orders/delivery-method/delivery-method';
import { CartItem } from '../../../shared/models/carts/cart-item';
import { Shoe } from '../../../shared/models/shoes/shoe';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private readonly baseUrl = `${environment.apiUrl}/carts`;
  readonly cartKey = 'usr.cartKey';

  cart = signal<ShoppingCart | null>(null);
  cartId = signal<string | null>(this.getCartId());
  selectedDelivery = signal<DeliveryMethod | null>(null);
  isEmpty = computed(() => {
    const cart = this.cart();
    return !cart || cart.items.length === 0;
  });
  itemCount = computed(() => {
    const cart = this.cart();
    if (!cart) return 0;
    return cart.items.reduce((sum, item) => sum + item.quantity, 0);
  });
  subtotal = computed(() => {
    const cart = this.cart();
    if (!cart) return 0;
    return cart.items.reduce((sum, item) => sum + (item.price * item.quantity), 0);
  });
  shipping = computed(() => {
    const delivery = this.selectedDelivery();
    return delivery ? delivery.price : 0;
  });
  totalAmount = computed(() => {
    return this.subtotal() + this.shipping();
  });

  constructor(
    private readonly http: HttpClient
  ) {}

  getById(id: string): Observable<ShoppingCart> {
    return this.http.get<ShoppingCart>(`${this.baseUrl}/${id}`).pipe(
      tap(cart => {
        this.cart.set(cart);
      })
    );
  }

  addItemToCart(item: CartItem | Shoe, quantity = 1): Observable<void> {
    const cart = this.cart();
    if (!cart) return of(undefined);

    if (this.isProduct(item)) {
      item = this.mapProductToCartItem(item);
    }

    let params = new HttpParams()
      .set('quantity', quantity);

    return this.http.post<void>(`${this.baseUrl}/${cart.shoppingCartId}/items`, item, { params }).pipe(
      tap(() => {
        const index = cart.items.findIndex(i => i.productId === item.productId);
        if (index === -1) {
          item.quantity = quantity;
          cart.items.push(item);
        }
        else {
          cart.items[index].quantity += quantity;
        }
        this.cart.set({ ...cart });
      })
    );
  }

  removeItemFromCart(productId: string, quantity = 1): Observable<void> {
    const cart = this.cart();
    if (!cart) return of(undefined);

    let params = new HttpParams()
      .set('quantity', quantity);

    return this.http.delete<void>(`${this.baseUrl}/${cart.shoppingCartId}/items/${productId}`, { params }).pipe(
      tap(() => {
        const index = cart.items.findIndex(i => i.productId === productId);
        if (index === -1) return;

        if (cart.items[index].quantity > quantity) {
          cart.items[index].quantity -= quantity;
        }
        else {
          cart.items.splice(index, 1);
        }

        if (cart.items.length === 0) {
          this.delete(cart.shoppingCartId).subscribe();
        }
        else {
          this.cart.set({ ...cart });
        }
      })
    );
  }

  clearItems(): Observable<void> {
    const cart = this.cart();
    if (!cart) return of(undefined);

    return this.http.delete<void>(`${this.baseUrl}/${cart.shoppingCartId}/items`).pipe(
      tap(() => {
        cart.items = [];
        this.cart.set({ ...cart });
      })
    );
  }

  create(): Observable<ShoppingCart> {
    return this.http.post<ShoppingCart>(this.baseUrl, {}).pipe(
      tap(cart => {
        console.log(cart);
        this.cart.set({ ...cart });
        this.setCartId(cart.shoppingCartId);
      })
    );
  }

  update(cart: ShoppingCart): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${cart.shoppingCartId}`, cart).pipe(
      tap(() => this.cart.set({ ...cart }))
    );
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`).pipe(
      tap(() => {
        this.cart.set(null);
        this.removeCartId();
      })
    );
  }

  productQuantityInCart(productId: string): number {
    const cart = this.cart();
    if (!cart) return 0;

    const item = cart.items.find(i => i.productId === productId);
    return item ? item.quantity : 0;
  }

  private mapProductToCartItem(item: Shoe): CartItem {
    return {
      productId: item.shoeId,
      productName: item.name,
      price: item.price,
      quantity: 1,
      imageUrl: item.images.find(i => i.isMain)!.url,
      brand: item.brand.name,
      category: item.category.name
    }
  }

  private isProduct(item: CartItem | Shoe): item is Shoe {
    return (item as Shoe).shoeId !== undefined;
  }

  private getCartId(): string | null {
    return localStorage.getItem(this.cartKey);
  }

  private setCartId(cartId: string): void {
    localStorage.setItem(this.cartKey, cartId);
    this.cartId.set(cartId);
  }

  private removeCartId(): void {
    localStorage.removeItem(this.cartKey);
    this.cartId.set(null);
  }
}
