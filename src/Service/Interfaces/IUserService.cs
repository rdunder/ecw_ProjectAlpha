

using Service.Dtos;
using Service.Models;

namespace Service.Interfaces;

public interface IUserService : IService<UserModel, UserDto>
{
    public Task<bool> AddToRoleAsync(UserModel model, string roleName);
    public Task<UserModel> GetByEmailAsync(string email);
}
