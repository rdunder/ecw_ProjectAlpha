

using Service.Dtos;
using Service.Interfaces;
using Service.Models;

namespace Service.Services;

public class RoleService : IRoleService
{
    public Task<bool> CreateAsync(RoleDto? dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<RoleModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<RoleModel> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Guid id, RoleDto? dto)
    {
        throw new NotImplementedException();
    }
}
