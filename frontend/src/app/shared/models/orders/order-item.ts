import { Shoe } from "../shoes/shoe";

export interface OrderItem {
    shoe: Shoe;
    price: number;
    quantity: number;
}