import { BrandCreate } from "./brand-create";

export interface BrandUpdate extends BrandCreate {
    brandId: string;
}