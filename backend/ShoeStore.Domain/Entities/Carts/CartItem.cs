using ShoeStore.Domain.Entities.Shoes;

namespace ShoeStore.Domain.Entities.Carts;

public partial class CartItem
{
    public Guid ShoppingCartId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Shoe Product { get; set; } = null!;

    public virtual ShoppingCart ShoppingCart { get; set; } = null!;
}