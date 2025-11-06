import { Routes } from "@angular/router";
import { CategoriesComponent } from "./categories.component";
import { CategoryFormComponent } from "./category-form/category-form.component";

export const categoriesRoutes: Routes = [
    { path: '', component: CategoriesComponent },
    { path: 'new', component: CategoryFormComponent },
    { path: 'edit/:id', component: CategoryFormComponent },
];