import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { DeliveryMethod } from '../../../shared/models/orders/delivery-method/delivery-method';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeliveryMethodService {
  private readonly baseUrl = `${environment.apiUrl}/orders/deliveryMethods`;

  constructor(
    private readonly http: HttpClient
  ) {}

  getAll(): Observable<DeliveryMethod[]> {
    return this.http.get<DeliveryMethod[]>(this.baseUrl);
  }

  getById(id: number): Observable<DeliveryMethod> {
    return this.http.get<DeliveryMethod>(`${this.baseUrl}/${id}`);
  }
}
