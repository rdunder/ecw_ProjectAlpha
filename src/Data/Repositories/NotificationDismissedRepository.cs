

using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class NotificationDismissedRepository(AppDbContext context) :
    BaseRepository<NotificationDismissedEntity>(context), INotificationDismissedRepository
{
}
