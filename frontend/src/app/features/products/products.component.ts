import { Component, OnInit } from '@angular/core';
import { ShoeService } from '../../core/services/shoes/shoe.service';
import { PagedList } from '../../shared/models/paged-list';
import { Shoe } from '../../shared/models/shoes/shoe';
import { ProductCardComponent } from './product-card/product-card.component';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIcon } from '@angular/material/icon';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { MatDialog } from '@angular/material/dialog';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state.component';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { ShoeQuery } from '../../shared/models/shoes/shoe-query';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-products',
  imports: [
    MatPaginator,
    MatMenuModule,
    MatIcon,
    MatButtonModule,
    ProductCardComponent,
    EmptyStateComponent,
    MatListOption,
    MatSelectionList,
    FormsModule
  ],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent implements OnInit {
  products?: PagedList<Shoe>;
  query = new ShoeQuery();
  sortOptions = [
    { label: 'Alphabetical', value: { sortBy: 'name', sortDirection: 'asc' } },
    { label: 'Price: Low to High', value: { sortBy: 'price', sortDirection: 'asc' } },
    { label: 'Price: High to Low', value: { sortBy: 'price', sortDirection: 'desc' } },
    { label: 'Newest', value: { sortBy: 'createdAt', sortDirection: 'desc' } },
  ];

  constructor(
    private readonly shoeService: ShoeService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly dialog: MatDialog,
  ) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      const searchTerm = params['text'];
      if (searchTerm) {
        this.query.searchTerm = searchTerm;
      }
      this.getProducts();
    });
  }

  handlePageEvent(event: PageEvent): void {
    this.query.pageNumber = event.pageIndex + 1;
    this.query.pageSize = event.pageSize;
    this.getProducts();
  }

  openFiltersDialog(): void {
    const dialogRef = this.dialog.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: this.query.brands,
        selectedCategories: this.query.categories
      }
    });

    dialogRef.afterClosed().subscribe(
      result => {
        if (result) {
          this.query.brands = result.selectedBrands;
          this.query.categories = result.selectedCategories;
          this.resetProducts();
        }
      }
    );
  }

  onSortChange(event: MatSelectionListChange): void {
    const selectedOption = event.options[0].value;
    this.query.sortBy = selectedOption.sortBy;
    this.query.sortDirection = selectedOption.sortDirection;
    this.resetProducts();
  }

  resetFilters(): void {
    this.query = new ShoeQuery();
    this.getProducts();
  }

  private resetProducts(): void {
    this.query.pageNumber = 1;
    this.getProducts();
  }

  private getProducts() {
    this.shoeService.getAll(this.query).subscribe(
      products => this.products = products
    );
  }
}
