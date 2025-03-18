

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

        //await _roleManager.CreateAsync(new RoleEntity() { Name = "Chief Executive Officer" });
        //await _roleManager.CreateAsync(new RoleEntity() { Name = "Administrator" });
        //await _roleManager.CreateAsync(new RoleEntity() { Name = "Chief Technician Officer" });
        //await _roleManager.CreateAsync(new RoleEntity() { Name = "Frontend Developer" });
        //await _roleManager.CreateAsync(new RoleEntity() { Name = "Fullstack Developer" });
        //await _roleManager.CreateAsync(new RoleEntity() { Name = "Backend Developer" });
        //await _roleManager.CreateAsync(new RoleEntity() { Name = "Trainee" });

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

    public async Task<bool> DeleteAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
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

    public async Task<IEnumerable<User>> GetAllAsync()
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

    public Task<User> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(int id, UserDto? dto)
    {
        throw new NotImplementedException();
    }
}
