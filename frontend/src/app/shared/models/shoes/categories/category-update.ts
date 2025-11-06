import { CategoryCreate } from "./category-create";

export interface CategoryUpdate extends CategoryCreate {
    categoryId: string;
}