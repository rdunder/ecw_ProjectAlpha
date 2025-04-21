using Data.Enums;

namespace Service.Dtos;

public class NotificationDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Message { get; set; } = null!;
    public DateTime Created { get; set; } = DateTime.Now;
    public string Icon { get; set; } = null!;
    public NotificationTargetGroup TargetGroup { get; set; }
    public NotificationType Type { get; set; }
    public string? TypeName { get; set; }
}
