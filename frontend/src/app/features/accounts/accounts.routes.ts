import { Routes } from "@angular/router";
import { ProfileComponent } from "./profile/profile.component";
import { AddressFormComponent } from "./address-form/address-form.component";

export const accountsRoutes: Routes = [
    { path: 'profile', component: ProfileComponent },
    { path: 'profile/address', component: AddressFormComponent },
];