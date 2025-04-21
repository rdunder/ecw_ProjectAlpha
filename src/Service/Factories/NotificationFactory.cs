using Data.Entities;
using Service.Dtos;
using Service.Models;

namespace Service.Factories;
public static class NotificationFactory
{
    public static NotificationDto Create() => new NotificationDto();

    public static NotificationEntity Create(NotificationDto dto) =>
        dto is null
        ? throw new ArgumentNullException(nameof(dto))
        : new NotificationEntity
        {
            Id = dto.Id,
            Message = dto.Message,
            TargetGroup = dto.TargetGroup,
            Type = dto.Type,
            Icon = dto.Icon,
            Created = dto.Created,
        };

    public static NotificationModel Create(NotificationEntity entity) =>
        entity is null
        ? throw new ArgumentNullException(nameof(entity))
        : new NotificationModel
        {
            Id = entity.Id,
            Message = entity.Message,
            Created = entity.Created,
            TargetGroup = entity.TargetGroup,
            Type = entity.Type,
            Icon = entity.Icon,
        };
}
