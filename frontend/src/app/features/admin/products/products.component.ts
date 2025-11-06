import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { RouterLink } from '@angular/router';
import { PagedList } from '../../../shared/models/paged-list';
import { Shoe } from '../../../shared/models/shoes/shoe';
import { ShoeService } from '../../../core/services/shoes/shoe.service';
import { ShoeQuery } from '../../../shared/models/shoes/shoe-query';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { CurrencyPipe } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';

@Component({
  selector: 'app-products',
  imports: [
    CurrencyPipe,
    RouterLink,
    FormsModule,
    MatFormFieldModule,
    MatButton,
    MatIcon,
    MatMenuModule,
    MatPaginator,
    MatInput
  ],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent implements OnInit {
  products = new PagedList<Shoe>();
  query = new ShoeQuery();

  constructor(
    private readonly shoeService: ShoeService,
  ) {}

  ngOnInit(): void {
    this.getProducts();
  }

  getMainImage(product: Shoe): string {
    const mainExistingImage = product.images.find(img => img.isMain);
    if (mainExistingImage) {
      return mainExistingImage.url;
    } else if (product.images.length > 0) {
      return product.images[0].url;
    } else {
      return 'images/no-image.png';
    }
  }

  deleteProduct(id: string) {
    this.shoeService.delete(id).subscribe(() =>{
      this.products.items = this.products.items.filter(x => x.shoeId !== id);
      this.products.totalCount--;
    })
  }

  onSearchChange(): void {
    this.query.pageNumber = 1;
    this.getProducts();
  }

  handlePageEvent(event: PageEvent): void {
    this.query.pageNumber = event.pageIndex + 1;
    this.getProducts();
  }

  private getProducts() {
    this.shoeService.getAll(this.query).subscribe(
      products => this.products = products
    );
  }
}
