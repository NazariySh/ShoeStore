import { Address } from "./address";

export interface User {
    userId: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    birthDate?: Date;
    address?: Address;
    roles: string[];
}