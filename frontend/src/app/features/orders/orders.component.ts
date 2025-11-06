import { Component, OnInit } from '@angular/core';
import { Order } from '../../shared/models/orders/order';
import { OrderService } from '../../core/services/orders/order.service';
import { RouterLink } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { PagedList } from '../../shared/models/paged-list';
import { OrderQuery } from '../../shared/models/orders/order-query';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { FormsModule } from '@angular/forms';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import { DateFormatPipe } from '../../shared/pipes/date-format.pipe';

@Component({
  selector: 'app-orders',
  imports: [
    CommonModule,
    RouterLink,
    FormsModule,
    MatFormField,
    MatLabel,
    MatIcon,
    MatPaginator,
    MatInput,
    DateFormatPipe
  ],
  providers: [DatePipe],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent implements OnInit {
  orders = new PagedList<Order>();
  query = new OrderQuery();

  constructor(
    private orderService: OrderService,
  ) {}

  ngOnInit(): void {
    this.getOrders();
  }
  
  onSearchChange(): void {
    this.query.pageNumber = 1;
    this.getOrders();
  }

  handlePageEvent(event: PageEvent): void {
    this.query.pageNumber = event.pageIndex + 1;
    this.getOrders();
  }

  getOrders(): void {
    this.orderService.getAllForAccount(this.query).subscribe(
      orders => this.orders = orders
    );
  }
}
