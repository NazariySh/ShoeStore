import { Routes } from "@angular/router";
import { ProductsComponent } from "./products.component";
import { ProductFormComponent } from "./product-form/product-form.component";

export const productsRoutes: Routes = [
    { path: '', component: ProductsComponent },
    { path: 'new', component: ProductFormComponent },
    { path: 'edit/:id', component: ProductFormComponent },
    {
        path: 'brands',
        loadChildren: () => import('./brands/brands.routes').then(r => r.brandsRoutes),
    },
    {
        path: 'categories',
        loadChildren: () => import('./categories/categories.routes').then(r => r.categoriesRoutes),
    }
];