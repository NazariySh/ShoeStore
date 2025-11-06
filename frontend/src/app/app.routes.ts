import { Routes } from '@angular/router';
import { adminGuard } from './core/guards/admin.guard';
import { authGuard } from './core/guards/auth.guard';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { ServerErrorComponent } from './shared/components/server-error/server-error.component';
import { emptyCartGuard } from './core/guards/empty-cart.guard';

export const routes: Routes = [
    { path: '', redirectTo: 'shop', pathMatch: 'full' },
    {
        path: 'shop',
        loadChildren: () => import('./features/products/products.routes').then(r => r.productRoutes)
    },
    {
        path: 'shopping-cart',
        loadChildren: () => import('./features/cart/cart.routes').then(r => r.cartRoutes)
    },
    {
        path: 'auth',
        loadChildren: () => import('./features/auth/auth.routes').then(r => r.authRoutes)
    },
    {
        path: 'orders',
        loadChildren: () => import('./features/orders/orders.routes').then(r => r.ordersRoutes),
        canActivate: [authGuard]
    },
    {
        path: 'checkout',
        loadChildren: () => import('./features/checkout/checkout.routes').then(r => r.checkoutRoutes),
        canActivate: [authGuard, emptyCartGuard]
    },
    {
        path: 'account',
        loadChildren: () => import('./features/accounts/accounts.routes').then(r => r.accountsRoutes),
        canActivate: [authGuard]
    },
    {
        path: 'admin',
        loadChildren: () => import('./features/admin/admin.routes').then(c => c.adminRoutes),
        canActivate: [authGuard, adminGuard]
    },
    { path: 'not-found', component: NotFoundComponent },
    { path: 'server-error', component: ServerErrorComponent },
    { path: '**', redirectTo: 'not-found', pathMatch: 'full' },
];
