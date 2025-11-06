namespace ShoeStore.Application.DTOs.Carts;

public class CartItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string Category { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; } = null!;
}