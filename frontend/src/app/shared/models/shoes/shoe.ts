import { Brand } from "./brands/brand";
import { Category } from "./categories/category";
import { ShoeImage } from "./shoe-images/shoe-image";

export interface Shoe {
    shoeId: string;
    name: string;
    description: string;
    price: number;
    category: Category;
    brand: Brand;
    sku: string;
    stock: number;
    totalSold: number;
    createdAt: Date;
    images: ShoeImage[];
}