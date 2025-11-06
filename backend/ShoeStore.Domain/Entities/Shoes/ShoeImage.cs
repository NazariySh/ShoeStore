namespace ShoeStore.Domain.Entities.Shoes;

public partial class ShoeImage
{
    public Guid ShoeImageId { get; set; }

    public string Url { get; set; } = null!;

    public string PublicId { get; set; } = null!;

    public bool IsMain { get; set; }

    public Guid ShoeId { get; set; }

    public virtual Shoe Shoe { get; set; } = null!;
}
