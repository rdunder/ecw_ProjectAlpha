

using Data.Interfaces;
using Service.Dtos;
using Service.Factories;
using Service.Interfaces;
using Service.Models;

namespace Service.Services;

public class StatusService(IStatusRepository repo) : IStatusService
{
    private readonly IStatusRepository _repo = repo;

    public async Task<bool> CreateAsync(StatusDto? dto)
    {
        if (dto == null) return false;

        await _repo.BeginTransactionAsync();

        try
        {
            var entity = StatusFactory.Create(dto);

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

    public async Task<IEnumerable<StatusModel>> GetAllAsync()
    {
        var statuses = new List<StatusModel>();

        try
        {
            var entities = await _repo.GetAllAsync();
            statuses.AddRange(entities.Select(entity => StatusFactory.Create(entity)));

            return statuses;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public async Task<StatusModel> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await _repo.GetAsync(x => x.Id == id);

            if (entity != null)
                return StatusFactory.Create(entity);

            return null!;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public async Task<bool> UpdateAsync(Guid id, StatusDto? dto)
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
