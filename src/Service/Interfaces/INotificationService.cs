using Service.Dtos;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces;

public interface INotificationService
{
    public Task<bool> CreateNotificationAsync(NotificationDto dto);
    public Task<IEnumerable<NotificationModel>> GetNotificationsAsync(Guid userId, int take = 10);
    public Task DismissNotificationAsync(Guid notificationId, Guid userId);
}
