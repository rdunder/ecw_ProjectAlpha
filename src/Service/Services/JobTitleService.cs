using Data.Interfaces;
using Service.Dtos;
using Service.Factories;
using Service.Interfaces;
using Service.Models;

namespace Service.Services;
public class JobTitleService(IJobTitleRepository jobTitleRepository) : IJobTitleService
{
    private readonly IJobTitleRepository _repo = jobTitleRepository;
    public async Task<bool> CreateAsync(JobTitleDto? dto)
    {
        if (dto == null) return false;

        await _repo.BeginTransactionAsync();

        try
        {
            var entity = JobTitleFactory.Create(dto);

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

    public async Task<IEnumerable<JobTitleModel>> GetAllAsync()
    {
        var titles = new List<JobTitleModel>();

        try
        {
            var entities = await _repo.GetAllAsync();
            titles.AddRange(entities.Select(entity => JobTitleFactory.Create(entity)));

            return titles;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public async Task<JobTitleModel> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await _repo.GetAsync(x => x.Id == id);

            if (entity != null)
                return JobTitleFactory.Create(entity);

            return null!;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public async Task<JobTitleModel> GetByTitleNameAsync(string titleName)
    {
        try
        {
            var entity = await _repo.GetAsync(x => x.Title == titleName);

            if (entity != null)
                return JobTitleFactory.Create(entity);

            return null!;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public async Task<bool> UpdateAsync(Guid id, JobTitleDto? dto)
    {
        if (dto is null || await _repo.AlreadyExistsAsync(x => x.Id == dto.Id) == false) return false;
        await _repo.BeginTransactionAsync();

        try
        {
            var entity = JobTitleFactory.Create(dto);
            entity.Id = id;
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

    public async Task<bool> Exists(string titleName)
    {
        return await _repo.AlreadyExistsAsync(x => x.Title == titleName);
    }
}
