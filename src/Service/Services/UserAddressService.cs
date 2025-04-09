

using Data.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Dtos;
using Service.Factories;
using Service.Interfaces;
using Service.Models;

namespace Service.Services;

public class UserAddressService(IUserAddressRepository repo) : IUserAddressService
{
    private readonly IUserAddressRepository _repo = repo;

    public async Task<bool> CreateAsync(UserAddressDto? dto)
    {
        if (dto == null) return false;

        await _repo.BeginTransactionAsync();

        try
        {
            var entity = UserAddressFactory.Create(dto);

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

    public async Task<IEnumerable<UserAddressModel>> GetAllAsync()
    {
        var addresses = new List<UserAddressModel>();

        try
        {
            var entities = await _repo.GetAllAsync();
            addresses.AddRange(entities.Select(entity => UserAddressFactory.Create(entity)));

            return addresses;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public async Task<UserAddressModel> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await _repo.GetAsync(x => x.UserEntityId == id);

            if (entity != null)
                return UserAddressFactory.Create(entity);

            return null!;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public async Task<bool> UpdateAsync(Guid id, UserAddressDto? dto)
    {
        if (dto == null) return false;

        await _repo.BeginTransactionAsync();

        try
        {
            var entity = UserAddressFactory.Create(dto);            

            if (await _repo.AlreadyExistsAsync(a => a.UserEntityId == id) == false)
            {
                await _repo.CreateAsync(entity);                
            }
            else
            {
                entity.UserEntityId = id;
                _repo.Update(entity);
            }

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
        if (await _repo.AlreadyExistsAsync(x => x.UserEntityId == id) == false) return false;

        await _repo.BeginTransactionAsync();

        try
        {
            var entity = await _repo.GetAsync(x => x.UserEntityId == id);
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
