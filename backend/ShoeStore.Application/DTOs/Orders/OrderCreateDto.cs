namespace ShoeStore.Application.DTOs.Orders;

public class OrderCreateDto
{
    public Guid ShoppingCartId { get; set; }
    public Guid DeliveryMethodId { get; set; }
}