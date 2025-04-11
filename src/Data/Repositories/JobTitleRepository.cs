

using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class JobTitleRepository(AppDbContext context) :
    BaseRepository<JobTitleEntity>(context), IJobTitleRepository
{
}
