import { Routes } from "@angular/router";
import { OrderDetailsComponent } from "./order-details/order-details.component";
import { OrdersComponent } from "./orders.component";

export const ordersRoutes: Routes = [
    { path: '', component: OrdersComponent },
    { path: ':id', component: OrderDetailsComponent }
];