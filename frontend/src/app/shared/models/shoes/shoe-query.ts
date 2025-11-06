export class ShoeQuery {
    brands: string[] = [];
    categories: string[] = [];
    pageNumber = 1;
    pageSize = 10;
    sortBy: string | null = null;
    sortDirection: string | null = null;
    searchTerm: string | null = null;
}