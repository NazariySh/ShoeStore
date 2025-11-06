import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Category } from '../../../shared/models/shoes/categories/category';
import { CategoryCreate } from '../../../shared/models/shoes/categories/category-create';
import { CategoryUpdate } from '../../../shared/models/shoes/categories/category-update';
import { Observable } from 'rxjs';
import { PagedList } from '../../../shared/models/paged-list';
import { PaginationQuery } from '../../../shared/models/pagination-query';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private readonly baseUrl = `${environment.apiUrl}/shoes/categories`;

  constructor(
    private readonly http: HttpClient
  ) {}

  getAllPaged(query: PaginationQuery): Observable<PagedList<Category>> {
    let params = new HttpParams()
      .set('pageNumber', query.pageNumber)
      .set('pageSize', query.pageSize);

    return this.http.get<PagedList<Category>>(`${this.baseUrl}/paged`, { params });
  }

  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(this.baseUrl);
  }

  getById(id: string): Observable<Category> {
    return this.http.get<Category>(`${this.baseUrl}/${id}`);
  }

  create(category: CategoryCreate): Observable<Category> {
    return this.http.post<Category>(this.baseUrl, category);
  }

  update(category: CategoryUpdate): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}`, category);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
