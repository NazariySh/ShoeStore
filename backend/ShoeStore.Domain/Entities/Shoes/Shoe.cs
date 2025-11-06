using ShoeStore.Domain.Entities.Carts;
using ShoeStore.Domain.Entities.Orders;

namespace ShoeStore.Domain.Entities.Shoes;

public partial class Shoe
{
    public Guid ShoeId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public Guid CategoryId { get; set; }

    public Guid BrandId { get; set; }

    public string Sku { get; set; } = null!;

    public int Stock { get; set; }

    public int TotalSold { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ShoeImage> ShoeImages { get; set; } = new List<ShoeImage>();
}
