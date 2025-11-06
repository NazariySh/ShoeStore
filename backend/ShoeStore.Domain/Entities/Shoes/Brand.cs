namespace ShoeStore.Domain.Entities.Shoes;

public partial class Brand
{
    public Guid BrandId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Shoe> Shoes { get; set; } = new List<Shoe>();
}
