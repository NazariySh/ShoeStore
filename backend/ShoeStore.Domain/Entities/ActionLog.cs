using ShoeStore.Domain.Entities.Users;

namespace ShoeStore.Domain.Entities;

public partial class ActionLog
{
    public Guid LogId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime ActionTime { get; set; }

    public string ActionType { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public Guid RecordId { get; set; }

    public string? Description { get; set; }

    public virtual User? User { get; set; }
}

