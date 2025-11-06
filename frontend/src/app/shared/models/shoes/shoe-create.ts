import { ShoeImageCreate } from "./shoe-images/shoe-image-create";

export interface ShoeCreate {
    name: string;
    description: string;
    price: number;
    categoryId: string;
    brandId: string;
    sku: string;
    stock: number;
    images: ShoeImageCreate[];
}