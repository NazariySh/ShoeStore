import { Component, OnInit } from '@angular/core';
import { Order } from '../../../shared/models/orders/order';
import { OrderService } from '../../../core/services/orders/order.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { AuthService } from '../../../core/services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-order-details',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule
  ],
  templateUrl: './order-details.component.html',
  styleUrl: './order-details.component.scss'
})
export class OrderDetailsComponent implements OnInit {
  order?: Order;
  id?: number;

  get buttonText() {
    return this.authService.isAdmin() ? 'Return to admin' : 'Return to orders';
  }

  get totalPrice(): number {
    if (!this.order) {
      return 0;
    }
    return this.order.subtotal + this.order.shipping;
  }

  constructor(
    private readonly orderService: OrderService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.getOrder(id);
    }
  }

  getAddressInfo(order?: Order): string {
    const address = order?.customer?.address;
    if (!address) {
      return 'No address provided';
    }
    return `${address.street}, ${address.city}, ${address.state}, ${address.postalCode}, ${address.country}`;
  }

  onReturnClick() {
    this.authService.isAdmin() 
      ? this.router.navigateByUrl('/admin/orders')
      : this.router.navigateByUrl('/orders')
  }

  private getOrder(id: string) {
    this.orderService.getById(id).subscribe({
      next: order => {
        this.order = order;
        console.log(order);
      } 
    })
  }
}
