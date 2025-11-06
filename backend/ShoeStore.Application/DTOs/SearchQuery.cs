using Microsoft.AspNetCore.Mvc;

namespace ShoeStore.Application.DTOs;

public class SearchQuery
{
    private string? _searchTerm;

    [FromQuery(Name = "searchTerm")]
    public string? SearchTerm
    {
        get => _searchTerm;
        set => _searchTerm = value?.Trim().ToLowerInvariant();
    }
}