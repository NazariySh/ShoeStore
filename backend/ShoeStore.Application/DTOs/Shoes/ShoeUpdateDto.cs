namespace ShoeStore.Application.DTOs.Shoes;

public class ShoeUpdateDto : ShoeCreateUpdateDto
{
    public Guid ShoeId { get; set; }
    public ICollection<Guid> RemovedImageIds { get; set; } = [];
}