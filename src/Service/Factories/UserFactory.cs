

using Data.Entities;
using Service.Dtos;
using Service.Models;

namespace Service.Factories;

public static class UserFactory
{
    public static UserDto Create() => new UserDto();

    public static UserEntity Create(UserDto dto) =>
        dto is null
            ? throw new ArgumentNullException(nameof(dto))
            : new UserEntity()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.UserName ?? dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Avatar = dto.Avatar,
                BirthDate = dto.BirthDate,
            };

    public static UserModel Create(UserEntity entity) =>
        entity is null
            ? throw new ArgumentNullException(nameof(entity))
            : new UserModel()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email ?? "",
                UserName = entity.UserName,
                PhoneNumber = entity.PhoneNumber ?? "",
                RoleName = entity.RoleName,
                Avatar = entity.Avatar,
                BirthDate = entity.BirthDate,
            };

    public static UserEntity Create(UserModel model) =>
        model is null
        ? throw new ArgumentNullException(nameof(model))
        : new UserEntity()
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.UserName,
            PhoneNumber = model.PhoneNumber,
            RoleName = model.RoleName,
            Avatar = model.Avatar,
            BirthDate = model.BirthDate,
            
        };
}
