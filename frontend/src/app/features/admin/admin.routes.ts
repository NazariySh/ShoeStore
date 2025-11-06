import { Routes } from "@angular/router";

export const adminRoutes: Routes = [
    {
        path: 'products',
        loadChildren: () => import('./products/products.routes').then(r => r.productsRoutes),
    },
    {
        path: 'orders',
        loadChildren: () => import('./orders/orders.routes').then(r => r.ordersRoutes),
    }
];