namespace ShoeStore.Application.DTOs.Orders;

public class OrderQuery
{
    public PaginationQuery Pagination { get; set; } = new();
    public SearchQuery Search { get; set; } = new();
    public SortQuery Sort { get; set; } = new();
}