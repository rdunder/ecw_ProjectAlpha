

using Data.Interfaces;
using Service.Dtos;
using Service.Factories;
using Service.Interfaces;
using Service.Models;

namespace Service.Services;

public class CustomerService(ICustomerRepository repo) : ICustomerService
{
    private readonly ICustomerRepository _repo = repo;


    public async Task<bool> CreateAsync(CustomerDto? dto)
    {
        if (dto == null) return false;

        if (await _repo.AlreadyExistsAsync(x => x.Email == dto.Email))
            return false;

        await _repo.BeginTransactionAsync();

        try
        {
            var entity = CustomerFactory.Create(dto);

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

    public async Task<IEnumerable<CustomerModel>> GetAllAsync()
    {
        var customers = new List<CustomerModel>();

        try
        {
            var entities = await _repo.GetAllAsync();
            customers.AddRange(entities.Select(entity => CustomerFactory.Create(entity)));

            return customers;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public async Task<CustomerModel> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await _repo.GetAsync(x => x.Id == id);

            if (entity != null)
                return CustomerFactory.Create(entity);

            return null!;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public async Task<bool> UpdateAsync(Guid id, CustomerDto? dto)
    {
        if (dto is null || await _repo.AlreadyExistsAsync(x => x.Id == dto.Id) == false) return false;
        await _repo.BeginTransactionAsync();

        try
        {
            var entity = CustomerFactory.Create(dto);
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
            var entity = await _repo.GetAsync(x =>x.Id == id);
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
