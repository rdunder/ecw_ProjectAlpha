using Data.Enums;

namespace Service.Models;

public class NotificationModel
{
    public Guid Id { get; set; }
    public string Message { get; set; } = null!;
    public DateTime Created { get; set; }
    public NotificationTargetGroup TargetGroup { get; set; }
    public NotificationType Type { get; set; }
    public string TypeName => Type.ToString();
    public string Icon { get; set; } = null!;
}
