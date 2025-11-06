using Microsoft.AspNetCore.Mvc;

namespace ShoeStore.Application.DTOs.Shoes;

public class ShoeQuery
{
    private ICollection<string> _categories = [];
    private ICollection<string> _brands = [];

    [FromQuery(Name = "categories")]
    public ICollection<string> Categories
    {
        get => _categories;
        set
        {
            _categories = value.SelectMany(
                x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToArray();
        }
    }

    [FromQuery(Name = "brands")]
    public ICollection<string> Brands
    {
        get => _brands;
        set
        {
            _brands = value.SelectMany(
                x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToArray();
        }
    }

    [FromQuery(Name = "pagination")]
    public PaginationQuery Pagination { get; set; } = new();

    [FromQuery(Name = "search")]
    public SearchQuery Search { get; set; } = new();

    [FromQuery(Name = "sort")]
    public SortQuery Sort { get; set; } = new();
}