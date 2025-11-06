namespace ShoeStore.Domain.Models;

public class PagedList<T>
{
    public List<T> Items { get; set; } = [];
    public ushort PageNumber { get; set; }
    public ushort PageSize { get; set; }
    public ushort TotalCount { get; set; }
    public ushort TotalPages { get; set; }

    public PagedList()
    {
    }

    public PagedList(List<T> items, ushort pageNumber, ushort pageSize, ushort totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (ushort)Math.Ceiling(totalCount / (double)pageSize);
    }
}