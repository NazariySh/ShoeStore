import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of, tap } from 'rxjs';
import { Address } from '../../../shared/models/accounts/address';
import { User } from '../../../shared/models/accounts/user';
import { ResetPassword } from '../../../shared/models/accounts/reset-password';
import { environment } from '../../../../environments/environment';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private readonly baseUrl = `${environment.apiUrl}/accounts`;

  constructor(
    private readonly http: HttpClient,
    private readonly authService: AuthService
  ) {}

  deleteProfile(): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/profile`).pipe(
      tap(() => {
        this.authService.currentUser.set(null);
      })
    );
  }

  getProfile(): Observable<User | null> {
    return this.http.get<User>(`${this.baseUrl}/profile`).pipe(
      tap(user => this.authService.currentUser.set(user)),
      catchError(error => {
        if (error.status === 401) {
          console.warn('User is not authenticated.');
          return of(null);
        }

        console.error('Error fetching user profile:', error);
        return of(null);
      })
    );
  }

  updateAddress(address: Address): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/address`, address).pipe(
      tap(() => {
        const user = this.authService.currentUser();
        if (user) {
          user.address = address;
          this.authService.currentUser.set({...user});
        }
      })
    );
  }

  resetPassword(resetPassword: ResetPassword): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/reset-password`, resetPassword).pipe(
      tap(() => {
        this.authService.logout().subscribe();
      })
    );
  }
}
