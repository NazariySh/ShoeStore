import { computed, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, throwError } from 'rxjs';
import { Login } from '../../../shared/models/auth/login';
import { Token } from '../../../shared/models/auth/token';
import { Register } from '../../../shared/models/auth/register';
import { environment } from '../../../../environments/environment';
import { User } from '../../../shared/models/accounts/user';
import { RoleType } from '../../../shared/enums/role-type';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl = `${environment.apiUrl}/auth`;
  private readonly accessTokenKey = 'usr.sessionKey';

  currentUser = signal<User | null>(null);
  accessToken = signal<string | null>(this.getAccessToken());
  isAdmin = computed(() => {
    const roles = this.currentUser()?.roles;
    return roles?.includes(RoleType[RoleType.Admin]) ?? false;
  });
  isLoggedIn = computed(() => {
    return this.accessToken() != null && this.currentUser() != null;
  });

  constructor(
    private readonly http: HttpClient
  ) {}

  login(login: Login): Observable<Token> {
    return this.http.post<Token>(`${this.baseUrl}/login`, login).pipe(
      tap(token => {
        this.saveToken(token)
      })
    );
  }

  register(register: Register): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/register`, register);
  }

  registerWithRole(register: Register, roleName: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/register/${roleName}`, register);
  }

  refresh(): Observable<Token> {
    const accessToken = this.accessToken();
    if (accessToken) {
      return this.http.post<Token>(`${this.baseUrl}/refresh-token`, accessToken).pipe(
        tap(token => {
          this.saveToken(token)
        })
      );
    }

    return throwError(() => 'No tokens found for refresh');
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/logout`, null).pipe(
      tap(() => {
        this.currentUser.set(null);
        this.clearToken();
      })
    );
  }

  private getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  private saveToken(token: Token) {
    localStorage.setItem(this.accessTokenKey, token.accessToken);
    this.accessToken.set(token.accessToken);
  }

  private clearToken() {
    localStorage.removeItem(this.accessTokenKey);
    this.accessToken.set(null);
  }
}
