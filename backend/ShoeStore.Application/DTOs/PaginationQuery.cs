using Microsoft.AspNetCore.Mvc;

namespace ShoeStore.Application.DTOs;

public class PaginationQuery
{
    private const ushort DefaultPageNumber = 1;
    private const ushort DefaultPageSize = 10;

    private ushort _pageNumber = DefaultPageNumber;
    private ushort _pageSize = DefaultPageSize;

    [FromQuery(Name = "pageNumber")]
    public ushort PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? DefaultPageNumber : value;
    }

    [FromQuery(Name = "pageSize")]
    public ushort PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? DefaultPageSize : value;
    }
}