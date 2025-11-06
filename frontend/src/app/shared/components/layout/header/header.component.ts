import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatDivider } from '@angular/material/divider';
import { MatIcon } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressBar } from '@angular/material/progress-bar';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth/auth.service';
import { MatBadgeModule } from '@angular/material/badge';
import { CartService } from '../../../../core/services/carts/cart.service';
import { BusyService } from '../../../../core/services/busy.service';
import { FormsModule } from '@angular/forms';
import { ShoeService } from '../../../../core/services/shoes/shoe.service';
import { IsAdminDirective } from '../../../directives/is-admin.directive';

@Component({
  selector: 'app-header',
  imports: [
    RouterLink,
    IsAdminDirective,
    RouterLinkActive,
    FormsModule,
    MatIcon,
    MatButton,
    MatMenuModule,
    MatDivider,
    MatBadgeModule,
    MatProgressBar
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  searchTerm = '';

  constructor(
    private readonly router: Router,
    private readonly authService: AuthService,
    private readonly cartService: CartService,
    private readonly busyService: BusyService
  ) {}

  get isLoggedIn() {
    return this.authService.isLoggedIn;
  }

  get currentUser() {
    return this.authService.currentUser;
  }

  get isAdmin() {
    return this.authService.isAdmin;
  }

  get isLoading() {
    return this.busyService.loading;
  }

  get cartItems() {
    return this.cartService.itemCount;
  }

  onSearchChange(): void {
    this.router.navigate(['/shop/search'], {
      queryParams: { text: this.searchTerm }
    });
  }

  logout(): void {
    this.authService.logout().subscribe(
      () => this.router.navigateByUrl('/auth/login')
    );
  }
}
