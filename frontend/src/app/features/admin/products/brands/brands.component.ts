import { Component, OnInit } from '@angular/core';
import { Brand } from '../../../../shared/models/shoes/brands/brand';
import { BrandService } from '../../../../core/services/shoes/brand.service';
import { RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIcon } from '@angular/material/icon';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { PaginationQuery } from '../../../../shared/models/pagination-query';
import { PagedList } from '../../../../shared/models/paged-list';

@Component({
  selector: 'app-brands',
  imports: [
    RouterLink,
    MatButton,
    MatIcon,
    MatMenuModule,
    MatPaginator
  ],
  templateUrl: './brands.component.html',
  styleUrl: './brands.component.scss'
})
export class BrandsComponent implements OnInit {
  brands = new PagedList<Brand>();
  query = new PaginationQuery();

  constructor(
    private readonly brandService: BrandService
  ) {}

  ngOnInit(): void {
    this.getBrands();
  }

  deleteBrand(id: string): void {
    this.brandService.delete(id).subscribe(() => {
      this.brands.items = this.brands.items.filter(brand => brand.brandId !== id);
      this.brands.totalCount--;
    });
  }

  handlePageEvent(event: PageEvent): void {
    this.query.pageNumber = event.pageIndex + 1;
    this.getBrands();
  }

  private getBrands(): void {
    this.brandService.getAllPaged(this.query).subscribe(
      brands => this.brands = brands
    );
  }
}
