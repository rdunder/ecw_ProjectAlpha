

using Data.Interfaces;
using Service.Dtos;
using Service.Factories;
using Service.Interfaces;
using Service.Models;

namespace Service.Services;

public class ProjectService(IProjectRepository repo) : IProjectService
{
    private readonly IProjectRepository _repo = repo;
    public async Task<bool> CreateAsync(ProjectDto? dto)
    {
        if (dto == null) return false;

        await _repo.BeginTransactionAsync();

        try
        {
            var entity = ProjectFactory.Create(dto);

            await _repo.CreateAsync(entity);
            await _repo.SaveChangesAsync();
            await _repo.CommitTransactionAsync();
            return true;
        }
        catch (Exception ex)
        {
            await _repo.RollbackTransactionAsync();
            return false;
        }
    }   

    public async Task<IEnumerable<ProjectModel>> GetAllAsync()
    {
        var projects = new List<ProjectModel>();

        try
        {
            var entities = await _repo.GetAllAsync();
            projects.AddRange(entities.Select(entity => ProjectFactory.Create(entity)));

            return projects;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public async Task<ProjectModel> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await _repo.GetAsync(x => x.Id == id);

            if (entity != null)
                return ProjectFactory.Create(entity);

            return null!;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public Task<bool> UpdateAsync(Guid id, ProjectDto? dto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (await _repo.AlreadyExistsAsync(x => x.Id == id) == false) return false;

        await _repo.BeginTransactionAsync();

        try
        {
            var entity = await _repo.GetAsync(x => x.Id == id);
            _repo.Delete(entity);

            await _repo.SaveChangesAsync();
            await _repo.CommitTransactionAsync();
            return true;
        }
        catch (Exception ex)
        {
            await _repo.RollbackTransactionAsync();
            return false;
        }
    }
}
