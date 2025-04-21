using Data.Entities;
using Service.Dtos;
using Service.Models;

namespace Service.Factories;
public static class CustomerFactory
{
    public static CustomerDto Create() => new CustomerDto();

    public static CustomerEntity Create(CustomerDto dto) =>
        dto is null
        ? throw new ArgumentNullException(nameof(dto))
        : new CustomerEntity()
        {
            CustomerName = dto.CustomerName,
            Email = dto.Email
        };

    public static CustomerModel Create(CustomerEntity entity) =>
        entity is null
        ? throw new ArgumentNullException(nameof(entity))
        : new CustomerModel()
        {
            Id = entity.Id,
            CustomerName = entity.CustomerName,
            Email = entity.Email,
        };

    public static CustomerEntity Create(CustomerModel model) =>
        model is null
        ? throw new ArgumentNullException(nameof(model))
        : new CustomerEntity()
        {
            Id = model.Id,
            CustomerName = model.CustomerName,
            Email = model.Email,
        };
}
