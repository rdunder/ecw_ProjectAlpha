

using Service.Dtos;
using Service.Models;
using System.Security.Claims;

namespace Service.Interfaces;

public interface IUserService
{
    public Task<bool> CreateAsync(UserDto? dto);
    public Task<IEnumerable<User>> GetAllAsync();
    public Task<User> GetByIdAsync(int id);
    public Task<bool> UpdateAsync(int id, UserDto? dto);
    public Task<bool> DeleteAsync(string email);
}
