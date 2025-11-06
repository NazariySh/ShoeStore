namespace ShoeStore.Application.DTOs.Orders.DeliveryMethods;

public class DeliveryMethodDto
{
    public Guid DeliveryMethodId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
}