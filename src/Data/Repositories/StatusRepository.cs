using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;
public class StatusRepository(AppDbContext context) :
    BaseRepository<StatusEntity>(context), IStatusRepository
{
}
