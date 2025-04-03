

using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Dtos;
using Service.Factories;
using Service.Interfaces;
using Service.Models;

namespace Service.Services;

public class ProjectService(IProjectRepository repo, IStatusRepository statusRepo, UserManager<UserEntity> usermManager) : IProjectService
{
    private readonly IProjectRepository _repo = repo;
    private readonly IStatusRepository _statusRepo = statusRepo;
    private readonly UserManager<UserEntity> _usermManager = usermManager;

    public async Task<bool> CreateAsync(ProjectDto? dto)
    {
        if (dto == null) return false;        

        await _repo.BeginTransactionAsync();

        try
        {
            var entity = ProjectFactory.Create(dto);

            if (dto.Users != null)
            {
                foreach (var user in dto.Users)
                {
                    var userEntity = await _usermManager.FindByIdAsync(user.Id.ToString());
                    if (userEntity != null)
                    {
                        entity.Users.Add(userEntity);
                    }
                }
            }
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
            var entities = await _repo.GetAllAsync(query => 
                query
                    .Include(x => x.Status)
                    .Include(x => x.Customer)
                    .Include(x => x.Users)
                );

            foreach (var entity in entities)
            {
                if (entity.Status.StatusName == "Pending")
                    await CheckStatus(entity);
            }

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
            var entity = await _repo.GetAsync(x => x.Id == id, query =>
                query
                    .Include(x => x.Status)
                    .Include(x => x.Customer)
                    .Include(x => x.Users)
               );

            if (entity != null)
                return ProjectFactory.Create(entity);

            return null!;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public async Task<bool> UpdateAsync(Guid id, ProjectDto? dto)
    {
        if (dto is null || await _repo.AlreadyExistsAsync(x => x.Id == dto.Id) == false) return false;

        await _repo.BeginTransactionAsync();

        try
        {
            var entity = ProjectFactory.Create(dto);
            entity.Id = dto.Id;

            _repo.Update(entity);

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

    public async Task<bool> StartProjectAsync(Guid id)
    {
        var entity = await _repo.GetAsync(p => p.Id == id);
        var statusEntity = await _statusRepo.GetAsync(s => s.StatusName == "Active");

        if (entity == null) return false;

        entity.StatusId = statusEntity.Id;
        entity.StartDate = DateOnly.FromDateTime(DateTime.Now);

        await _repo.BeginTransactionAsync();
        try
        {
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            await _repo.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _repo.RollbackTransactionAsync();
            return false;
        }
    }

    public async Task<bool> CloseProjectAsync(Guid id)
    {
        var entity = await _repo.GetAsync(p => p.Id == id);
        var statusEntity = await _statusRepo.GetAsync(s => s.StatusName == "Closed");

        if (entity == null) return false;

        entity.StatusId = statusEntity.Id;
        entity.EndDate = DateOnly.FromDateTime(DateTime.Now);

        await _repo.BeginTransactionAsync();

        try
        {
            _repo.Update(entity);

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

    public async Task<bool> UpdateMemberList(List<Guid> memberIds, Guid projectId)
    {
        if (memberIds == null || projectId == Guid.Empty) return false;

        var projectEntity = await _repo.GetAsync(p => p.Id == projectId, query =>
                query
                    .Include(x => x.Status)
                    .Include(x => x.Customer)
                    .Include(x => x.Users)
               );

        await _repo.BeginTransactionAsync();
        projectEntity.Users.Clear();       

        await _repo.SaveChangesAsync();

        foreach (var id in memberIds)
        {
            var user = await _usermManager.FindByIdAsync(id.ToString());
            if (user != null && projectEntity.Users.Contains(user) == false)
            {
                projectEntity.Users.Add(user);
            }
        }

        try
        {
            _repo.Update(projectEntity);
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

    private async Task CheckStatus(ProjectEntity entity)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        if (entity.StartDate <= today)
        {
            await StartProjectAsync(entity.Id);
        }
    }
}
