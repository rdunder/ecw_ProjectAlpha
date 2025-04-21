using Data.Entities;
using Service.Dtos;
using Service.Models;

namespace Service.Factories;
public static class RoleFactory
{
    public static RoleDto Create() => new RoleDto();

    public static RoleEntity Create(RoleDto dto) =>
       dto is null
       ? throw new ArgumentNullException(nameof(dto))
       : new RoleEntity()
       {
           Name = dto.RoleName
       };

    public static RoleModel Create(RoleEntity entity) =>
        entity is null
        ? throw new ArgumentNullException(nameof(entity))
        : new RoleModel()
        {
            Id = entity.Id,
            Name = entity.Name ?? ""
        };

    public static RoleEntity Create(RoleModel model) =>
        model is null
        ? throw new ArgumentNullException(nameof(model))
        : new RoleEntity()
        {
            Id = model.Id,
            Name = model.Name
        };
}
