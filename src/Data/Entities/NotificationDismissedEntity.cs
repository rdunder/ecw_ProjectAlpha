using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class NotificationDismissedEntity
{
    [Key]
    public int Id { get; set; }

    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public Guid NotificationId { get; set; }
    public NotificationEntity Notification { get; set; } = null!;
}