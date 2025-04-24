using Service.Dtos;
using Service.Models;

namespace Service.Interfaces;
public interface INotificationService
{
    public Task<bool> CreateNotificationAsync(NotificationDto dto);
    public Task<IEnumerable<NotificationModel>> GetNotificationsAsync(Guid userId, int take = 10);
    public Task DismissNotificationAsync(Guid notificationId, Guid userId);
    public Task<IEnumerable<NotificationModel>> GetAllAsync();
}
