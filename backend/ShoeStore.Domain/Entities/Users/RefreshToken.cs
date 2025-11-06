namespace ShoeStore.Domain.Entities.Users;

public partial class RefreshToken
{
    public Guid UserId { get; set; }

    public string? Token { get; set; }

    public DateTime? ExpiryTime { get; set; }

    public virtual User User { get; set; } = null!;
}
