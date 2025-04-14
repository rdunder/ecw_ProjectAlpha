using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories;

public class NotificationRepository(AppDbContext context) :
    BaseRepository<NotificationEntity>(context), INotificationRepository
{
}
