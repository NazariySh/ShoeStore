import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BrandService } from '../../../../../core/services/shoes/brand.service';
import { BaseFormComponent } from '../../../../../shared/components/base/base-form-component';
import { FormFieldComponent } from '../../../../../shared/components/form-fields/form-field/form-field.component';
import { MatButton } from '@angular/material/button';
import { BrandCreate } from '../../../../../shared/models/shoes/brands/brand-create';
import { BrandUpdate } from '../../../../../shared/models/shoes/brands/brand-update';

@Component({
  selector: 'app-brand-form',
  imports: [
    FormFieldComponent,
    ReactiveFormsModule,
    MatButton
  ],
  templateUrl: './brand-form.component.html',
  styleUrl: './brand-form.component.scss'
})
export class BrandFormComponent extends BaseFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  readonly form = this.fb.group({
    name: ['', [Validators.required]],
  });
  isEditMode = false;
  id?: string;

  get formText(): string {
    return this.isEditMode ? 'Update Brand' : 'Create Brand';
  }

  constructor(
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute,
    private readonly brandService: BrandService
  ) {
    super();
  }

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.getBrand(id);
    }
  }

  onSubmit(): void {
    if (this.isEditMode) {
      this.updateBrand()
    }
    else {
      this.createBrand()
    }
  }

  private createBrand(): void {
    const brand = this.mapFromToBrandCreate();
    if (!brand) {
      return;
    }

    this.brandService.create(brand).subscribe({
      next: () => {
        this.resetForm();
        this.router.navigateByUrl('/admin/products/brands');
      },
      error: this.handleFormErrors
    });
  }

  private updateBrand(): void {
    const brand = this.mapFromToBrandUpdate();
    if (!brand) {
      return;
    }

    this.brandService.update(brand).subscribe({
      next: () => {
        this.resetForm();
        this.router.navigateByUrl('/admin/products/brands');
      },
      error: this.handleFormErrors
    });
  }

  private mapFromToBrandCreate(): BrandCreate | null {
    const { name } = this.form.value;
    if (!name) {
      return null;
    }

    return { name };
  }

  private mapFromToBrandUpdate() : BrandUpdate | null {
    const { name } = this.form.value;
    if (!name || !this.id) {
      return null;
    }

    return { brandId: this.id, name };
  }

  private resetForm(): void {
    this.form.reset();
    this.isEditMode = false;
    this.id = undefined;
  }

  private getBrand(id : string): void {
    this.brandService.getById(id).subscribe(
      brand => {
        this.form.patchValue(brand);
        this.isEditMode = true;
        this.id = brand.brandId;
      }
    );
  }
}
