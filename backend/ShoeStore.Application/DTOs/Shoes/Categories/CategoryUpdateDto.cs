namespace ShoeStore.Application.DTOs.Shoes.Categories;

public class CategoryUpdateDto : CategoryCreateUpdateDto
{
    public Guid CategoryId { get; set; }
}