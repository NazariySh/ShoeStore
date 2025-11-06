export class OrderQuery {
  pageNumber = 1;
  pageSize = 10;
  searchTerm: string | null = null;
  sortBy: string | null = null;
  sortDirection: string | null = null;
}