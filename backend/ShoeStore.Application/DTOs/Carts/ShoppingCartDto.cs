using ShoeStore.Domain.Entities.Orders;

namespace ShoeStore.Application.DTOs.Carts;

public class ShoppingCartDto
{
    public Guid ShoppingCartId { get; set; }
    public DeliveryMethod? DeliveryMethod { get; set; }
    public IList<CartItemDto> Items { get; set; } = [];
}