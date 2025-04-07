

using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Dtos;
using Service.Factories;
using Service.Interfaces;
using Service.Models;
using System.Security.Claims;

namespace Service.Services;

public class UserService(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager, IUserAddressService userAddressService) : IUserService
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<RoleEntity> _roleManager = roleManager;
    private readonly IUserAddressService _userAddressService = userAddressService;


    public async Task<bool> CreateAsync(UserDto? dto)
    {
        if (dto == null) return false;
        var entity = UserFactory.Create(dto);

        if (dto.Address != null)
            entity.Address = UserAddressFactory.Create(dto.Address);

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

    public async Task<UserEntity> CreateExternalAsync(UserDto? dto, ExternalLoginInfo loginInfo)
    {
        if (dto == null) return null!;

        var entity = UserFactory.Create(dto);

        try
        {
            var result = await _userManager.CreateAsync(entity);
            if (result.Succeeded)
            {
                await _userManager.AddLoginAsync(entity, loginInfo);
                await _userManager.AddToRoleAsync(entity, "Trainee");
                return entity;
            }
            return null!;
        }
        catch (Exception ex) 
        {            
            return null!;
        }
        

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
        var userEntities = await _userManager.Users.Include(u => u.Address).ToListAsync();

        foreach (var userEntity in userEntities)
        {
            var roles = await _userManager.GetRolesAsync(userEntity);
            userEntity.RoleName = roles.FirstOrDefault() ?? string.Empty;
        }

        var users = userEntities.Select(entity => UserFactory.Create(entity));
        return users;
    }

    public async Task<UserModel> GetByIdAsync(Guid id)
    {
        var entity = await _userManager.FindByIdAsync(id.ToString());
        return entity is null ? null! : UserFactory.Create(entity);
    }


    public async Task<bool> UpdateAsync(Guid id, UserDto? dto)
    {
        if (dto is null) return false;
        var entity = await _userManager.FindByIdAsync(id.ToString());

        if (entity != null)
        {
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Email = dto.Email;
            entity.Avatar = dto.Avatar;
            entity.PhoneNumber = dto.PhoneNumber;
            
            if (entity.RoleName != dto.RoleName && dto.RoleName != null)
                await AddToRoleAsync(dto.Email, dto.RoleName);

            if (dto.Address != null)
                await _userAddressService.UpdateAsync(id, dto.Address);

            var result = await _userManager.UpdateAsync(entity);
            return result.Succeeded;
        }

        return false;        
    }

    public async Task<bool> AddToRoleAsync(string email, string roleName)
    {
        if (email == null) return false;

        var entity = await _userManager.FindByEmailAsync(email);
        if (entity == null) return false;


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
        var entity = await _userManager.FindByEmailAsync(email);
        
        return entity is null ? null! : UserFactory.Create(entity);
    }

    public async Task<UserModel> GetUser(ClaimsPrincipal claimsPrincipal)
    {
        var entity = await _userManager.GetUserAsync(claimsPrincipal);

        if (entity == null) return null!;
        
        var roles = await _userManager.GetRolesAsync(entity);
        entity.RoleName = roles.FirstOrDefault() ?? string.Empty;

        return entity is null ? null! : UserFactory.Create(entity);
    }

    public async Task<string> GetDisplayName(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        return user is null ? "Display Name" : $"{user.FirstName} {user.LastName}";
    }
}
