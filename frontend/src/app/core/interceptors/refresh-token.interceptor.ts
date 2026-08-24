import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, share, switchMap, throwError } from 'rxjs';
import { StatusCodes } from '../../shared/models/status-codes';
import { AuthService } from '../services/auth/auth.service';
import { Token } from '../../shared/models/auth/token';

let refreshInProgress$: Observable<Token> | null = null;

export const refreshTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (req.url.includes('/auth/refresh-token')) {
    return next(req);
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === StatusCodes.Status401Unauthorized) {
        if (!refreshInProgress$) {
          refreshInProgress$ = authService.refresh().pipe(share());
        }

        return refreshInProgress$.pipe(
          switchMap((response) => {
            req = req.clone({
              setHeaders: {
                Authorization: `Bearer ${response.accessToken}`
              }
            });

            return next(req);
          }),
          catchError((err) => {
            authService.clearSession();
            router.navigate(['/auth/login']);
            return throwError(() => err);
          })
        );
      }

      return throwError(() => error);
    })
  );
};
