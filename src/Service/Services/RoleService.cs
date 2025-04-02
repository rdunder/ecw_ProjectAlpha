

using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Dtos;
using Service.Factories;
using Service.Interfaces;
using Service.Models;

namespace Service.Services;

public class RoleService(RoleManager<RoleEntity> roleManager, AppDbContext context) : IRoleService
{
    private readonly AppDbContext _context = context;
    private readonly RoleManager<RoleEntity> _roleManager = roleManager;

    public async Task<bool> CreateAsync(RoleDto? dto)
    {
        if (dto == null) return false;

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var entity = RoleFactory.Create(dto);
                var result = await _roleManager.CreateAsync(entity);
                

                await transaction.CommitAsync();

                return result.Succeeded ? true : false;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }    

    public async Task<IEnumerable<RoleModel>> GetAllAsync()
    {
        var entities = await _roleManager.Roles.ToListAsync();
        var roles = entities.Select(entity => RoleFactory.Create(entity));
        
        return roles;
    }

    public Task<RoleModel> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Guid id, RoleDto? dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
