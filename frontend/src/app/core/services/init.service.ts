import { Injectable } from '@angular/core';
import { CartService } from './carts/cart.service';
import { AccountService } from './accounts/account.service';
import { forkJoin, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InitService {

  constructor(
    private readonly accountService: AccountService,
    private readonly cartService: CartService
  ) {}

  init() {
    const cartId = this.cartService.cartId();
    return forkJoin({
      cart: cartId ? this.cartService.getById(cartId) : this.cartService.create(),
      user: this.accountService.getProfile()
    })
  }
}
