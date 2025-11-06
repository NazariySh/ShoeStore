using ShoeStore.Domain.Entities.Users;

namespace ShoeStore.Domain.Entities.Orders;

public partial class Order
{
    public Guid OrderId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid? EmployeeId { get; set; }

    public string Status { get; set; } = null!;

    public Guid DeliveryMethodId { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Shipping { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual DeliveryMethod DeliveryMethod { get; set; } = null!;

    public virtual User? Employee { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

