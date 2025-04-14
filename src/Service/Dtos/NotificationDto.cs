

using Data.Entities;

namespace Service.Dtos;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Message { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public NotificationTargetGroup TargetGroup { get; set; }
    public NotificationType Type { get; set; }
    
}
