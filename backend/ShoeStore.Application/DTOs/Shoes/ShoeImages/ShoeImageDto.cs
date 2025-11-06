namespace ShoeStore.Application.DTOs.Shoes.ShoeImages;

public class ShoeImageDto
{
    public Guid ShoeImageId { get; set; }
    public string Url { get; set; } = null!;
    public string PublicId { get; set; } = null!;
    public bool IsMain { get; set; }
}