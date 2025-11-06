using ShoeStore.Domain.Entities.Orders;

namespace ShoeStore.Domain.Entities.Users;

public partial class User
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ActionLog> ActionLogs { get; set; } = new List<ActionLog>();

    public virtual Address? Address { get; set; }

    public virtual ICollection<Order> OrderCustomers { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderEmployees { get; set; } = new List<Order>();

    public virtual RefreshToken? RefreshToken { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
