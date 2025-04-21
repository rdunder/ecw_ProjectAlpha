using Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

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
