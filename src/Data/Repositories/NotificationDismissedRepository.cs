using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;
public class NotificationDismissedRepository(AppDbContext context) :
    BaseRepository<NotificationDismissedEntity>(context), INotificationDismissedRepository
{
    public async Task<List<Guid>> GetDismissedNotificationIdsAsync(Guid userId)
    {
        var ids = await _dbSet
            .Where(nd => nd.UserId == userId)
            .Select(nd => nd.NotificationId)
            .ToListAsync();

        return ids;
    }
}
