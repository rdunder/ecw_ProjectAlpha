using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities;

public enum NotificationTargetGroup
{
    All         = 0,
    Users       = 1,
    Managers    = 2,
    Admins      = 3
}

public class NotificationEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Message { get; set; } = null!;
    public DateTime Created { get; set; }

    public int TypeId { get; set; }
    public NotificationTypeEntity Type { get; set; }
}

public class NotificationTypeEntity
{
    [Key]
    public int Id { get; set; }
    public string Type { get; set; } = null!;
    public NotificationTargetGroup TargetGroup { get; set; }
    public string Icon { get; set; } = null!;

    public ICollection<NotificationEntity>? Notifications { get; set; }
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