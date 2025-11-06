import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { SnackbarService } from '../services/snackbar.service';
import { AuthService } from '../services/auth/auth.service';

export const adminGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const snackbar = inject(SnackbarService);

  if (authService.isAdmin()) {
    return true;
  }

  snackbar.error('Access denied: Admin privileges required.');
  router.navigateByUrl('/');
  return false;
};
