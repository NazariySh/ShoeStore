import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { RouterLink } from '@angular/router';
import { PagedList } from '../../../shared/models/paged-list';
import { Order } from '../../../shared/models/orders/order';
import { OrderQuery } from '../../../shared/models/orders/order-query';
import { OrderService } from '../../../core/services/orders/order.service';
import { OrderStatus } from '../../../shared/enums/order-status';
import { MatInput } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-orders',
  imports: [
    CommonModule,
    RouterLink,
    FormsModule,
    MatFormFieldModule,
    MatIcon,
    MatMenuModule,
    MatPaginator,
    MatInput,
    MatButtonModule
  ],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent {
  orders = new PagedList<Order>();
  query = new OrderQuery();

  constructor(
    private readonly orderService: OrderService,
  ) {}

  ngOnInit(): void {
    this.getOrders();
  }

  getAddressInfo(order: Order): string {
    const address = order?.customer?.address;
    if (!address) {
      return 'No address provided';
    }
    return `${address.street}, ${address.city}, ${address.state}, ${address.postalCode}, ${address.country}`;
  }

  onSearchChange(): void {
    this.query.pageNumber = 1;
    this.getOrders();
  }

  handlePageEvent(event: PageEvent): void {
    this.query.pageNumber = event.pageIndex + 1;
    this.getOrders();
  }

  approveOrder(id: string): void {
    this.updateStatus(id, OrderStatus.Processing)
  }

  rejectOrder(id: string): void {
    this.updateStatus(id, OrderStatus.Cancelled)
  }

  updateStatus(id: string, status: OrderStatus): void {
    this.orderService.updateStatus(id, status).subscribe({
      next: () => {
        const order = this.orders.items.find(x => x.orderId === id);
        if (order) {
          order.status = OrderStatus[status];
        }
      }
    });
  }

  private getOrders() {
    this.orderService.getAll(this.query).subscribe(
      orders => this.orders = orders,
    );
  }
}
