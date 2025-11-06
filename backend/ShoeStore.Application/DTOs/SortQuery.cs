using Microsoft.AspNetCore.Mvc;

namespace ShoeStore.Application.DTOs;

public class SortQuery
{
    public const string Ascending = "asc";
    public const string Descending = "desc";

    private string _sortDirection = Ascending;

    [FromQuery(Name = "sortBy")]
    public string? SortBy { get; set; }

    [FromQuery(Name = "sortDirection")]
    public string SortDirection
    {
        get => _sortDirection;
        set => _sortDirection = value.ToLower() switch
        {
            Ascending or Descending => value.ToLower(),
            _ => Ascending
        };
    }

    public bool IsAscendingOrder => SortDirection == Ascending;
    public bool IsDescendingOrder => SortDirection == Descending;
}