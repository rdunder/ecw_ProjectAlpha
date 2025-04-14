using Data.Entities;

namespace Data.Interfaces;

public interface INotificationDismissedRepository : IBaseRepository<NotificationDismissedEntity>
{
    public Task<List<Guid>> GetDismissedNotificationIdsAsync(Guid userId);
}
