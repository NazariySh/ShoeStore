import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { PagedList } from '../../../shared/models/paged-list';
import { Shoe } from '../../../shared/models/shoes/shoe';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ShoeCreate } from '../../../shared/models/shoes/shoe-create';
import { ShoeUpdate } from '../../../shared/models/shoes/shoe-update';
import { ShoeQuery } from '../../../shared/models/shoes/shoe-query';

@Injectable({
  providedIn: 'root'
})
export class ShoeService {
  private readonly baseUrl = `${environment.apiUrl}/shoes`;

  constructor(
    private readonly http: HttpClient
  ) {}

  getAll(query: ShoeQuery): Observable<PagedList<Shoe>> {
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

    if (query.brands?.length) {
      params = params.set('brands', query.brands.join(','));
    }
  
    if (query.categories?.length) {
      params = params.set('categories', query.categories.join(','));
    }

    return this.http.get<PagedList<Shoe>>(this.baseUrl, { params });
  }

  getById(id: string): Observable<Shoe> {
    return this.http.get<Shoe>(`${this.baseUrl}/${id}`);
  }

  create(shoe: ShoeCreate): Observable<string> {
    return this.http.post<string>(this.baseUrl, shoe);
  }

  update(shoe: ShoeUpdate): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}`, shoe);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
