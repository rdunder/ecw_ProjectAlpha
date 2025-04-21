using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;
public class NotificationRepository(AppDbContext context) :
    BaseRepository<NotificationEntity>(context), INotificationRepository
{
}
