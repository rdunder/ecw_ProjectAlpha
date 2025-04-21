using Data.Entities;
using Service.Dtos;
using Service.Models;

namespace Service.Factories;
public static class StatusFactory
{
    public static StatusDto Create() => new StatusDto();

    public static StatusEntity Create(StatusDto dto) =>
        dto is null
        ? throw new ArgumentNullException(nameof(dto))
        : new StatusEntity()
        {
            StatusName = dto.StatusName,
        };

    public static StatusModel Create(StatusEntity entity) =>
        entity is null
        ? throw new ArgumentNullException(nameof(entity))
        : new StatusModel()
        {
            Id = entity.Id,
            StatusName = entity.StatusName,
        };

    public static StatusEntity Create(StatusModel model) =>
        model is null
        ? throw new ArgumentNullException(nameof(model))
        : new StatusEntity()
        {
            Id = model.Id,
            StatusName = model.StatusName,
        };
}
