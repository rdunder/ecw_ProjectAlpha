

using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Dtos;
using Service.Models;
using System.Security.Claims;

namespace Service.Interfaces;

public interface IUserService : IService<UserModel, UserDto>
{
    public Task<bool> AddToRoleAsync(string email, string roleName);
    public Task<UserModel> GetByEmailAsync(string email);
    public Task<UserEntity> CreateExternalAsync(UserDto? dto, ExternalLoginInfo loginInfo);
    public Task<UserModel> GetUser(ClaimsPrincipal claimsPrincipal);
    public Task<string> GetDisplayName(ClaimsPrincipal claimsPrincipal);
    public Task<bool> UpdatePassword(Guid id, string currentPassword, string newPassword);
    public Task<bool> ConfirmEmail(Guid userId, string code);
}
