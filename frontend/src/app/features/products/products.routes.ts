import { Routes } from "@angular/router";
import { ProductsComponent } from "./products.component";
import { ProductDetailsComponent } from "./product-details/product-details.component";

export const productRoutes: Routes = [
    { path: '', component: ProductsComponent },
    { path: 'search', component: ProductsComponent },
    { path: ':id', component: ProductDetailsComponent },
];