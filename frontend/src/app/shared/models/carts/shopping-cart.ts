import { CartItem } from "./cart-item";

export interface ShoppingCart {
    shoppingCartId: string;
    deliveryMethodId?: string;
    items: CartItem[];
}