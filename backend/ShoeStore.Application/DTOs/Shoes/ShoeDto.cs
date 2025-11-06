using ShoeStore.Application.DTOs.Shoes.Brands;
using ShoeStore.Application.DTOs.Shoes.Categories;
using ShoeStore.Application.DTOs.Shoes.ShoeImages;

namespace ShoeStore.Application.DTOs.Shoes;

public class ShoeDto
{
    public Guid ShoeId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public CategoryDto Category { get; set; } = null!;
    public BrandDto Brand { get; set; } = null!;
    public string Sku { get; set; } = null!;
    public int Stock { get; set; }
    public int TotalSold { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<ShoeImageDto> Images { get; set; } = null!;
}