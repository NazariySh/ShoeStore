namespace ShoeStore.Application.DTOs.Shoes.Brands;

public class BrandUpdateDto : BrandCreateUpdateDto
{
    public Guid BrandId { get; set; }
}