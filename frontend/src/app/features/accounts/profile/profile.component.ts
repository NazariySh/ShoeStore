import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth/auth.service';
import { AccountService } from '../../../core/services/accounts/account.service';
import { User } from '../../../shared/models/accounts/user';
import { MatCardModule } from '@angular/material/card';
import { CommonModule, DatePipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { DateFormatPipe } from '../../../shared/pipes/date-format.pipe';

@Component({
  selector: 'app-profile',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIcon,
    DateFormatPipe,
    RouterLink
  ],
  providers: [DatePipe],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {

  constructor(
    private readonly router: Router,
    private readonly authService: AuthService,
    private readonly accountService: AccountService
  ) {}

  get user (): User | null {
    return this.authService.currentUser();
  }

  deleteProfile(): void {
    this.accountService.deleteProfile().subscribe(
      () => this.authService.logout().subscribe(
        () => this.router.navigateByUrl('/auth/login')
      )
    );
  }
}
