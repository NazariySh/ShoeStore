using ShoeStore.Application.DTOs.Shoes;

namespace ShoeStore.Application.DTOs.Orders;

public class OrderItemDto
{
    public ShoeDto Shoe { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}