import { Routes } from "@angular/router";
import { BrandsComponent } from "./brands.component";
import { BrandFormComponent } from "./brand-form/brand-form.component";

export const brandsRoutes: Routes = [
    { path: '', component: BrandsComponent },
    { path: 'new', component: BrandFormComponent },
    { path: 'edit/:id', component: BrandFormComponent },
];