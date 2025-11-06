using ShoeStore.Application.DTOs.Shoes.ShoeImages;

namespace ShoeStore.Application.DTOs.Shoes;

public class ShoeCreateUpdateDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    public string Sku { get; set; } = null!;
    public int Stock { get; set; }
    public ICollection<ShoeImageCreateDto> Images { get; set; } = [];
}