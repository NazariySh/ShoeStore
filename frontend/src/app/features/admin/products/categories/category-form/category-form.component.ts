import { Component, inject, OnInit } from '@angular/core';
import { FormFieldComponent } from '../../../../../shared/components/form-fields/form-field/form-field.component';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../../../../../core/services/shoes/category.service';
import { BaseFormComponent } from '../../../../../shared/components/base/base-form-component';
import { CategoryUpdate } from '../../../../../shared/models/shoes/categories/category-update';
import { CategoryCreate } from '../../../../../shared/models/shoes/categories/category-create';

@Component({
  selector: 'app-category-form',
  imports: [
    FormFieldComponent,
    ReactiveFormsModule,
    MatButton
  ],
  templateUrl: './category-form.component.html',
  styleUrl: './category-form.component.scss'
})
export class CategoryFormComponent extends BaseFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  readonly form = this.fb.group({
    name: ['', [Validators.required]],
  });
  isEditMode = false;
  id?: string;

  get formText(): string {
    return this.isEditMode ? 'Update Category' : 'Create Category';
  }

  constructor(
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute,
    private readonly categoryService: CategoryService
  ) {
    super();
  }

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.getCategory(id);
    }
  }

  onSubmit(): void {
    if (this.isEditMode) {
      this.updateCategory()
    }
    else {
      this.createCategory()
    }
  }

  private createCategory(): void {
    const category = this.mapFromToCategoryCreate();
    if (!category) {
      return;
    }

    this.categoryService.create(category).subscribe({
      next: () => {
        this.resetForm();
        this.router.navigateByUrl('/admin/products/categories');
      },
      error: this.handleFormErrors
    });
  }

  private updateCategory(): void {
    const category = this.mapFromToCategoryUpdate();
    if (!category) {
      return;
    }

    this.categoryService.update(category).subscribe({
      next: () => {
        this.resetForm();
        this.router.navigateByUrl('/admin/products/categories');
      },
      error: this.handleFormErrors
    });
  }

  private mapFromToCategoryCreate() : CategoryCreate | null {
    const { name } = this.form.value;
    if (!name) {
      return null;
    }

    return { name };
  }

  private mapFromToCategoryUpdate(): CategoryUpdate | null {
    const { name } = this.form.value;
    if (!name || !this.id) {
      return null;
    }

    return { categoryId: this.id, name };
  }

  private getCategory(id : string): void {
    this.categoryService.getById(id).subscribe(
      category => {
        this.form.patchValue(category);
        this.isEditMode = true;
        this.id = category.categoryId;
      }
    );
  }

  private resetForm(): void {
    this.form.reset();
    this.isEditMode = false;
    this.id = undefined;
  }
}
