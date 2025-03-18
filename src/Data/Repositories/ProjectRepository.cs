

using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

class ProjectRepository(AppDbContext context) :
    BaseRepository<ProjectEntity>(context), IProjectRepository
{
}
