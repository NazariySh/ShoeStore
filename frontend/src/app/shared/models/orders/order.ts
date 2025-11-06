import { OrderStatus } from '../../enums/order-status';
import { DeliveryMethod } from './delivery-method/delivery-method';
import { OrderItem } from './order-item';
import { User } from '../accounts/user';

export interface Order {
    orderId: string;
    customer: User;
    employee: User;
    status: string;
    deliveryMethod: DeliveryMethod;
    subtotal: number;
    shipping: number;
    createdAt: Date;
    orderItems: OrderItem[];
}