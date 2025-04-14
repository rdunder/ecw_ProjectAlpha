

using Data.Entities;
using Service.Dtos;

namespace Service.Factories;

public static class NotificationFactory
{
    public static NotificationDto Create() => new NotificationDto();

    public static NotificationEntity Create(NotificationDto dto) =>
        dto is null
        ? throw new ArgumentNullException(nameof(dto))
        : new NotificationEntity
        {
            Message = dto.Message,
            TargetGroup = dto.TargetGroup,
            Type = dto.Type,
            Icon = dto.Icon,
        };
}
