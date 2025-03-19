

using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Dtos;
using Service.Factories;
using Service.Interfaces;
using Service.Models;
using System.Security.Claims;

namespace Service.Services;

public class UserService(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager) : IUserService
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<RoleEntity> _roleManager = roleManager;


    public async Task<bool> CreateAsync(UserDto? dto)
    {
        var entity = UserFactory.Create(dto);
        var result = await _userManager.CreateAsync(entity, dto.Password);

        if (dto.RoleName == null)
        {
            await _userManager.AddToRoleAsync(entity, "Trainee");
        }
        else
        {
            await _userManager.AddToRoleAsync(entity, dto.RoleName);
        }

        return result.Succeeded ? true : false;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded ? true : false;
        }
        else
        {
            return false;
        }
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        var userEntities = await _userManager.Users.ToListAsync();

        foreach (var userEntity in userEntities)
        {
            var roles = await _userManager.GetRolesAsync(userEntity);
            userEntity.RoleName = roles.FirstOrDefault() ?? string.Empty;
        }

        var users = userEntities.Select(entity => UserFactory.Create(entity));
        return users;
    }

    public Task<UserModel> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Guid id, UserDto? dto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddToRoleAsync(UserModel model, string roleName)
    {
        if (model == null) return false;

        var entity = await _userManager.FindByEmailAsync(model.Email);


        var roles = await _userManager.GetRolesAsync(entity);
        foreach (var role in roles)
        {
            await _userManager.RemoveFromRoleAsync(entity, role);
        }


        if (await _roleManager.RoleExistsAsync(roleName) && entity != null)
        {
            var result = await _userManager.AddToRoleAsync(entity, roleName);
            return result.Succeeded;
        }

        return false;
    }

    public async Task<UserModel> GetByEmailAsync(string email)
    {
        return new UserModel();
    }
}
