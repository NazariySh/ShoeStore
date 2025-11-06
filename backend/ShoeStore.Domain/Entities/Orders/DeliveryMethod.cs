using ShoeStore.Domain.Entities.Carts;

namespace ShoeStore.Domain.Entities.Orders;

public partial class DeliveryMethod
{
    public Guid DeliveryMethodId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}

