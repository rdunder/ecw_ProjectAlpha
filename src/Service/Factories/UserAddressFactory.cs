using Data.Entities;
using Service.Dtos;
using Service.Models;

namespace Service.Factories;
public static class UserAddressFactory
{
    public static UserAddressDto Create() => new UserAddressDto();

    public static UserAddressEntity Create(UserAddressDto dto) =>
        dto is null
        ? throw new ArgumentNullException(nameof(dto))
        : new UserAddressEntity()
        {
            UserEntityId = dto.UserEntityId,
            Address = dto.Address,
            City = dto.City,
            PostalCode = dto.PostalCode,
        };

    public static UserAddressModel Create(UserAddressEntity entity) =>
        entity is null
        ? new UserAddressModel()
        : new UserAddressModel()
        {
            UserEntityId = entity.UserEntityId,
            Address = entity.Address,
            City = entity.City,
            PostalCode = entity.PostalCode,
        };

    public static UserAddressEntity Create(UserAddressModel model) =>
        model is null
        ? throw new ArgumentNullException(nameof(model))
        : new UserAddressEntity()
        {
            UserEntityId = model.UserEntityId,
            Address = model.Address,
            City = model.City,
            PostalCode = model.PostalCode,
        };
}
