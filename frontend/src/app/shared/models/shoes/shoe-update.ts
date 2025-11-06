import { ShoeCreate } from "./shoe-create";

export interface ShoeUpdate extends ShoeCreate {
    shoeId: string;
    removedImageIds: string[];
}