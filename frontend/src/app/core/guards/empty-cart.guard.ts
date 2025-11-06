import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CartService } from '../services/carts/cart.service';
import { SnackbarService } from '../services/snackbar.service';

export const emptyCartGuard: CanActivateFn = (route, state) => {
  const cartService = inject(CartService);
  const router = inject(Router);
  const snackbar = inject(SnackbarService);

  if (cartService.isEmpty()){
    snackbar.error('Your cart is empty');
    router.navigateByUrl('/shopping-cart');
    return false;
  }

  return true;
};
