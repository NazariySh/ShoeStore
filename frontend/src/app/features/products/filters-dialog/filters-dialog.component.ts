import { Component, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BrandService } from '../../../core/services/shoes/brand.service';
import { CategoryService } from '../../../core/services/shoes/category.service';
import { Brand } from '../../../shared/models/shoes/brands/brand';
import { Category } from '../../../shared/models/shoes/categories/category';
import { MatDivider } from '@angular/material/divider';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import { FormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-filters-dialog',
  imports: [
    CommonModule,
    FormsModule,
    MatSelectionList,
    MatListOption,
    MatDivider,
    MatButton
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})
export class FiltersDialogComponent implements OnInit {
  private readonly dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  private readonly data = inject(MAT_DIALOG_DATA);

  selectedBrands: string[] = this.data.selectedBrands || [];
  selectedCategories: string[] = this.data.selectedCategories || [];
  brands: Brand[] = [];
  categories: Category[] = [];

  constructor(
    private readonly brandService: BrandService,
    private readonly categoryService: CategoryService,
  ) {}

  ngOnInit(): void {
    this.getBrands();
    this.getCategories();
  }

  applyFilters(): void {
    this.dialogRef.close({
      selectedBrands: this.selectedBrands,
      selectedCategories: this.selectedCategories
    });
  }

  private getBrands(): void {
    this.brandService.getAll().subscribe(
      brands => this.brands = brands
    );
  }

  private getCategories(): void {
    this.categoryService.getAll().subscribe(
      categories => this.categories = categories
    );
  }
}
