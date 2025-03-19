

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
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Avatar = dto.Avatar,
                BirthDate = dto.BirthDate,
            };

    public static User Create(UserEntity entity) =>
        entity is null
            ? throw new ArgumentNullException(nameof(entity))
            : new User()
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
}
