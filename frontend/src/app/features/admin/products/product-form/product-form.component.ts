import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormFieldComponent } from '../../../../shared/components/form-fields/form-field/form-field.component';
import { TextareaFormFieldComponent } from '../../../../shared/components/form-fields/textarea-form-field/textarea-form-field.component';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ShoeService } from '../../../../core/services/shoes/shoe.service';
import { SnackbarService } from '../../../../core/services/snackbar.service';
import { BaseFormComponent } from '../../../../shared/components/base/base-form-component';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ShoeImage } from '../../../../shared/models/shoes/shoe-images/shoe-image';
import { SelectFormFieldComponent } from '../../../../shared/components/form-fields/select-form-field/select-form-field.component';
import { SelectItem } from '../../../../shared/models/select-item';
import { CategoryService } from '../../../../core/services/shoes/category.service';
import { BrandService } from '../../../../core/services/shoes/brand.service';

@Component({
  selector: 'app-product-form',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    SelectFormFieldComponent,
    FormFieldComponent,
    TextareaFormFieldComponent
  ],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.scss'
})
export class ProductFormComponent extends BaseFormComponent implements OnInit, OnDestroy {
  private readonly fb = inject(FormBuilder);
  form = this.fb.group({
    name: ['', [Validators.required]],
    description: ['', [Validators.required]],
    category: ['', [Validators.required]],
    brand: ['', [Validators.required]],
    sku: ['', [Validators.required]],
    price: [0, [Validators.required]],
    stock: [0, [Validators.required]],
  });
  existingImages: ShoeImage[] = [];
  newImages: File[] = [];
  removedImageIds: string[] = [];
  isEditMode = false;
  id?: string;
  allowedExtensions = ['jpg', 'jpeg', 'png'];
  categories: SelectItem<string>[] = [];
  brands: SelectItem<string>[] = [];
  private fileUrlCache = new Map<File, string>();

  constructor(
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute,
    private readonly snackbar: SnackbarService,
    private readonly shoeService: ShoeService,
    private readonly categoryService: CategoryService,
    private readonly brandService: BrandService
  ) {
    super();
  }

  get formText(): string {
    return this.isEditMode ? 'Update Product' : 'Create Product';
  }

  get mainImage(): string {
    const mainExistingImage = this.existingImages.find(img => img.isMain);
    if (mainExistingImage) {
      return mainExistingImage.url;
    } else if (this.newImages.length > 0) {
      const file = this.newImages[0];
      return this.imagePreview(file);
    } else {
      return 'images/no-image.png';
    }
  }

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.getProduct(id);
    }

    this.getCategories();
    this.getBrands();
  }

  ngOnDestroy(): void {
    this.fileUrlCache.forEach(url => URL.revokeObjectURL(url));
    this.fileUrlCache.clear();
  }
  
  imagePreview(file: File): string {
    if (!this.fileUrlCache.has(file)) {
      const objectUrl = URL.createObjectURL(file);
      this.fileUrlCache.set(file, objectUrl);
    }
    return this.fileUrlCache.get(file)!;
  }

  removeExistingImage(image: ShoeImage): void {
    this.existingImages = this.existingImages.filter(img => img.shoeImageId !== image.shoeImageId);
    this.removedImageIds.push(image.shoeImageId);
  }

  removeNewImage(file: File): void {
    this.newImages = this.newImages.filter(img => img !== file);

    const objectUrl = this.fileUrlCache.get(file);
    if (objectUrl) {
      URL.revokeObjectURL(objectUrl);
      this.fileUrlCache.delete(file);
    }
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) {
      this.snackbar.error('No files selected.');
      return;
    }

    this.uploadImages(input.files);
    input.value = '';
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    if (!event.dataTransfer?.files) return;

    this.uploadImages(event.dataTransfer.files);
  }

  onSubmit(): void {
    console.log(this.form.value);

    if (this.isEditMode) {
      this.updateProduct()
    }
    else {
      this.createProduct()
    }
  }

  onCancel(): void {
    this.resetForm();
    this.router.navigateByUrl('/admin/products');
  }

  private createProduct(): void {
    this.mapFormToShoeCreate().then((product) => {
      if (!product) {
        this.snackbar.error('Please fill in all required fields correctly.');
        return;
      }
  
      this.shoeService.create(product).subscribe({
        next: () => {
          this.resetForm();
          this.snackbar.success('Product created successfully!');
          this.router.navigateByUrl('/admin/products');
        },
        error: this.handleFormErrors,
      });
    });
  }

  private updateProduct(): void {
    this.mapFormToShoeUpdate().then((product) => {
      if (!product) {
        this.snackbar.error('Please fill in all required fields correctly.');
        return;
      }

      this.shoeService.update(product).subscribe({
        next: () => {
          this.resetForm();
          this.snackbar.success('Product updated successfully!');
          this.router.navigateByUrl('/admin/products');
        },
        error: this.handleFormErrors
      });
    });
  }

  private isImageFile(file: File): boolean {
    const fileExtension = file.name.split('.').pop()?.toLowerCase();
    return fileExtension ? this.allowedExtensions.includes(fileExtension) : false;
  }

  private uploadImages(files: FileList): void {
    Array.from(files).forEach(file => {
      if (this.isImageFile(file)) {
        this.newImages.push(file);
      } else {
        this.snackbar.error('Only image files (jpg, jpeg, png) are allowed.');
      }
    });
  }

  private async mapFormToShoeCreate() {
    const { name, description, category, brand, sku, stock, price } = this.form.value;
    if (!name || !description || !category || !brand || !sku || !stock || !price) {
      return null;
    }

    console.log(this.newImages);

    const images = await Promise.all(
      this.newImages.map(async (file) => {
        const base64 = await this.convertFileToBase64(file);
        return {
          file: base64,
          publicId: file.name,
          isMain: this.newImages.indexOf(file) === 0,
        };
      })
    );

    return {
      name,
      description,
      categoryId: category,
      brandId: brand,
      sku,
      stock,
      price,
      images: images
    };
  }

  private convertFileToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = (error) => reject(error);
      reader.readAsDataURL(file);
    });
  }

  private mapFormToShoeUpdate() {
    return this.mapFormToShoeCreate().then((product) => {
      if (!product) {
        return null;
      }
      return {
        shoeId: this.id!,
        removedImageIds: this.removedImageIds,
        ...product
      };
    })
  }

  private getProduct(id: string): void {
    this.shoeService.getById(id).subscribe(
      product => {
        this.form.patchValue({
          name: product.name,
          description: product.description,
          category: product.category.categoryId,
          brand: product.brand.brandId,
          sku: product.sku,
          stock: product.stock,
          price: product.price
        });
        this.isEditMode = true;
        this.id = product.shoeId;
        this.existingImages = product.images;
      }
    );
  }

  private getCategories() {
    this.categoryService.getAll().subscribe(
      categories => {
        this.categories = categories.map(category => ({
          label: category.name,
          value: category.categoryId
        }));
      }
    );
  }

  private getBrands() {
    this.brandService.getAll().subscribe(
      brands => {
        this.brands = brands.map(brand => ({
          label: brand.name,
          value: brand.brandId
        }));
      }
    );
  }

  private resetForm(): void {
    this.form.reset();
    this.existingImages = [];
    this.newImages = [];
    this.removedImageIds = [];
    this.isEditMode = false;
    this.id = undefined;
  }
}
