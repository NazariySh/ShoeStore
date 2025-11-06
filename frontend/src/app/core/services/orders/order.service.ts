import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Order } from '../../../shared/models/orders/order';
import { Observable } from 'rxjs';
import { OrderCreate } from '../../../shared/models/orders/order-create';
import { PagedList } from '../../../shared/models/paged-list';
import { OrderStatus } from '../../../shared/enums/order-status';
import { OrderQuery } from '../../../shared/models/orders/order-query';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private readonly baseUrl = `${environment.apiUrl}/orders`;

  constructor(
    private readonly http: HttpClient
  ) {}

  getAllForAccount(query: OrderQuery): Observable<PagedList<Order>> {
    let params = this.getQueryParams(query);
    return this.http.get<PagedList<Order>>(`${this.baseUrl}`, { params });
  }

  getAll(query: OrderQuery): Observable<PagedList<Order>> {
    let params = this.getQueryParams(query);
    return this.http.get<PagedList<Order>>(`${this.baseUrl}/admin`, { params });
  }

  getById(id: string): Observable<Order> {
    return this.http.get<Order>(`${this.baseUrl}/${id}`);
  }

  create(order: OrderCreate): Observable<string> {
    return this.http.post<string>(this.baseUrl, order);
  }

  updateStatus(id: string, status: OrderStatus): Observable<void> {
    let params = new HttpParams()
      .set('status', status);

    return this.http.patch<void>(`${this.baseUrl}/${id}`, { params });
  }

  private getQueryParams(query: OrderQuery): HttpParams {
    let params = new HttpParams()
      .set('pagination.pageNumber', query.pageNumber)
      .set('pagination.pageSize', query.pageSize);

    if (query.sortBy) {
      params = params
        .set('sort.sortBy', query.sortBy)
        .set('sort.sortDirection', query.sortDirection || 'asc');
    }

    if (query.searchTerm) {
      params = params.set('search.searchTerm', query.searchTerm);
    }

    return params;
  }
}
