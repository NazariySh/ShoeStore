using ShoeStore.Domain.Entities.Orders;

namespace ShoeStore.Domain.Entities.Carts;

public partial class ShoppingCart
{
    public Guid ShoppingCartId { get; set; }

    public Guid? DeliveryMethodId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual DeliveryMethod? DeliveryMethod { get; set; }
}
