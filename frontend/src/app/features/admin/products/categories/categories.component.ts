import { Component, OnInit } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { RouterLink } from '@angular/router';
import { CategoryService } from '../../../../core/services/shoes/category.service';
import { Category } from '../../../../shared/models/shoes/categories/category';
import { MatButton } from '@angular/material/button';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { PagedList } from '../../../../shared/models/paged-list';
import { PaginationQuery } from '../../../../shared/models/pagination-query';

@Component({
  selector: 'app-categories',
  imports: [
    RouterLink,
    MatIcon,
    MatMenuModule,
    MatButton,
    MatPaginator
  ],
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.scss'
})
export class CategoriesComponent implements OnInit {
  categories = new PagedList<Category>();
  query = new PaginationQuery();

  constructor(
    private readonly categoryService: CategoryService
  ) {}

  ngOnInit(): void {
    this.getCategories();
  }

  deleteCategory(id: string): void {
    this.categoryService.delete(id).subscribe(() => {
      this.categories.items = this.categories.items.filter(category => category.categoryId !== id);
      this.categories.totalCount--;
    });
  }

  handlePageEvent(event: PageEvent): void {
    this.query.pageNumber = event.pageIndex + 1;
    this.getCategories();
  }

  private getCategories(): void {
    this.categoryService.getAllPaged(this.query).subscribe(
      categories => this.categories = categories
    );
  }
}
