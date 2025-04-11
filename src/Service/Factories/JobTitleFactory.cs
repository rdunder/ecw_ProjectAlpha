

using Data.Entities;
using Service.Dtos;
using Service.Models;
using System.Runtime.InteropServices;

namespace Service.Factories;

public static class JobTitleFactory
{
    public static JobTitleDto Create() => new JobTitleDto();

    public static JobTitleEntity Create(JobTitleDto dto) =>
        dto is null
        ? throw new ArgumentNullException(nameof(dto))
        : new JobTitleEntity
        {
            Title = dto.Title,
        };

    public static JobTitleModel Create(JobTitleEntity entity) =>
        entity is null
        ? throw new ArgumentNullException(nameof(entity))
        : new JobTitleModel
        {
            Id = entity.Id,
            Title = entity.Title,
        };

    public static JobTitleEntity Create(JobTitleModel model) =>
        model is null
        ? throw new ArgumentNullException(nameof(model))
        : new JobTitleEntity
        {
            Id = model.Id,
            Title = model.Title,
        };
}
