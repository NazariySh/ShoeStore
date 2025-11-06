namespace ShoeStore.Application.DTOs.Shoes.ShoeImages;

public class ShoeImageCreateDto
{
    public string File { get; set; } = null!;
    public string PublicId { get; set; } = null!;
    public bool IsMain { get; set; }
}