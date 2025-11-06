import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { Brand } from '../../../shared/models/shoes/brands/brand';
import { Observable } from 'rxjs';
import { BrandCreate } from '../../../shared/models/shoes/brands/brand-create';
import { BrandUpdate } from '../../../shared/models/shoes/brands/brand-update';
import { PagedList } from '../../../shared/models/paged-list';
import { PaginationQuery } from '../../../shared/models/pagination-query';

@Injectable({
  providedIn: 'root'
})
export class BrandService {
  private readonly baseUrl = `${environment.apiUrl}/shoes/brands`;

  constructor(
    private readonly http: HttpClient
  ) {}

  getAllPaged(query: PaginationQuery): Observable<PagedList<Brand>> {
    let params = new HttpParams()
      .set('pageNumber', query.pageNumber)
      .set('pageSize', query.pageSize);

    return this.http.get<PagedList<Brand>>(`${this.baseUrl}/paged`, { params });
  }

  getAll(): Observable<Brand[]> {
    return this.http.get<Brand[]>(this.baseUrl);
  }

  getById(id: string): Observable<Brand> {
    return this.http.get<Brand>(`${this.baseUrl}/${id}`);
  }

  create(brand: BrandCreate): Observable<Brand> {
    return this.http.post<Brand>(this.baseUrl, brand);
  }

  update(brand: BrandUpdate): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}`, brand);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
