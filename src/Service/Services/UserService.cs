

using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Service.Dtos;
using Service.Factories;
using Service.Interfaces;
using Service.Models;
using System.Security.Claims;
using System.Text;

namespace Service.Services;

public class UserService(
    UserManager<UserEntity> userManager, 
    RoleManager<RoleEntity> roleManager, 
    IUserAddressService userAddressService,
    IJobTitleService jobTitleService,
    INotificationService notificationService) : IUserService
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<RoleEntity> _roleManager = roleManager;
    private readonly IUserAddressService _userAddressService = userAddressService;
    private readonly IJobTitleService _jobTitleService = jobTitleService;
    private readonly INotificationService _notificationService = notificationService;


    public async Task<bool> CreateAsync(UserDto? dto)
    {
        if (dto == null) return false;
        var entity = UserFactory.Create(dto);

        if (dto.Address != null)
            entity.Address = UserAddressFactory.Create(dto.Address);

        if (dto.JobTitleId == Guid.Empty)
        {
            if (await _jobTitleService.Exists("Guest") == false)
                await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Guest" });

            var title = await _jobTitleService.GetByTitleNameAsync("Guest");
            entity.JobTitleId = title != null ? title.Id : throw new ArgumentNullException("Job Title Guest is not precsent in the database");
        }

        var result = await _userManager.CreateAsync(entity, dto.Password);

        if (dto.RoleName == null || dto.JobTitleId == Guid.Empty)
        {
            await _userManager.AddToRoleAsync(entity, "Viewer");            
        }
        else
        {
            await _userManager.AddToRoleAsync(entity, dto.RoleName);
        }

        var allNotifications = await _notificationService.GetAllAsync();
        var user = await _userManager.FindByEmailAsync(dto.Email);
        foreach (var notification in allNotifications)
        {
           await _notificationService.DismissNotificationAsync(notification.Id, user.Id);
        }

        return result.Succeeded ? true : false;
    }

    public async Task<UserEntity> CreateExternalAsync(UserDto? dto, ExternalLoginInfo loginInfo)
    {
        if (dto == null) return null!;

        var entity = UserFactory.Create(dto);        

        try
        {
            if (await _jobTitleService.Exists("Guest") == false)
                await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Guest" });

            var title = await _jobTitleService.GetByTitleNameAsync("Guest");
            entity.JobTitleId = title != null ? title.Id : throw new ArgumentNullException("Job Title Guest is not precsent in the database");

            var result = await _userManager.CreateAsync(entity);
            if (result.Succeeded)
            {
                await _userManager.AddLoginAsync(entity, loginInfo);
                await _userManager.AddToRoleAsync(entity, "Viewer");
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
        var userEntities = await _userManager.Users.Include(u => u.Address).Include(u => u.JobTitle).OrderBy(u => u.LastName).ToListAsync();
        
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
        var entity = await _userManager.Users.Include(u => u.Address).Include(u => u.JobTitle).FirstOrDefaultAsync(u => u.Id == id);
        var roles = await _userManager.GetRolesAsync(entity);
        entity.RoleName = roles.FirstOrDefault() ?? string.Empty;
        return entity is null ? null! : UserFactory.Create(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, UserDto? dto)
    {
        if (dto is null) return false;

        var entity = await _userManager.Users
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (entity != null)
        {
            var roles = await _userManager.GetRolesAsync(entity);

            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Email = dto.Email;
            entity.Avatar = dto.Avatar;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.BirthDate = dto.BirthDate;
            entity.RoleName = roles.FirstOrDefault() ?? string.Empty;
            entity.JobTitleId = dto.JobTitleId;

            if (entity.RoleName != dto.RoleName && dto.RoleName != null)
                await AddToRoleAsync(dto.Email, dto.RoleName);

            if (entity.Address != null && dto.Address != null)
            {
                if (dto.Address.Address != null && dto.Address.PostalCode != null! && dto.Address.City != null)
                    entity.Address = UserAddressFactory.Create(dto.Address);
            }

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

    public async Task<bool> UpdatePassword(Guid id, string currentPassword, string newPassword)
    {
        var entity = await _userManager.FindByIdAsync(id.ToString());
        var result = await _userManager.ChangePasswordAsync(entity, currentPassword, newPassword);

        if (result.Succeeded)
            return true;

        return false;
    }
    
    public async Task<bool> ConfirmEmail(Guid userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        var result = await _userManager.ConfirmEmailAsync(user, code);
        return result.Succeeded;
    }

}
