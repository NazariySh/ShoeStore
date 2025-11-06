using ShoeStore.Application.DTOs.Accounts;
using ShoeStore.Application.DTOs.Orders.DeliveryMethods;

namespace ShoeStore.Application.DTOs.Orders;

public class OrderDto
{
    public Guid OrderId { get; set; }
    public UserDto Customer { get; set; } = null!;
    public UserDto Employee { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DeliveryMethodDto DeliveryMethod { get; set; } = null!;
    public decimal Subtotal { get; set; }
    public decimal Shipping { get; set; }
    public DateTime CreatedAt { get; set; }
    public IReadOnlyList<OrderItemDto> OrderItems { get; set; } = [];
}