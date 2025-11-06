using ShoeStore.Domain.Entities.Shoes;

namespace ShoeStore.Domain.Entities.Orders;

public partial class OrderItem
{
    public Guid OrderId { get; set; }

    public Guid ShoeId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Shoe Shoe { get; set; } = null!;
}

