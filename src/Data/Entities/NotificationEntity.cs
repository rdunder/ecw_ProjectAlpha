using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public enum NotificationType
{
    User = 1,
    Project = 2
}

public enum NotificationTargetGroup
{
    All         = 1,
    Users       = 2,
    Managers    = 3,
    Admins      = 4
}

public class NotificationEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Message { get; set; } = null!;
    public DateTime Created { get; set; }

    public NotificationTargetGroup TargetGroup { get; set; }
    public NotificationType Type { get; set; }

    public string Icon { get; set; } = null!;
}

public class NotificationDismissedEntity
{
    [Key]
    public int Id { get; set; }

    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public Guid NotificationId { get; set; }
    public NotificationEntity Notification { get; set; } = null!;
}