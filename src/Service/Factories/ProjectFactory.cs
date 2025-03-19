

using Data.Entities;
using Service.Dtos;
using Service.Models;

namespace Service.Factories;

public static class ProjectFactory
{
    public static ProjectDto Create() => new ProjectDto();

    public static ProjectEntity Create(ProjectDto dto) =>
        dto is null
        ? throw new ArgumentNullException(nameof(dto))
        : new ProjectEntity()
        {
            Name = dto.Name,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Budget = dto.Budget,
            Avatar = dto.Avatar,

            StatusId = dto.StatusId,
            CustomerId = dto.CustomerId,
        };

    public static ProjectModel Create(ProjectEntity entity) =>
        entity is null
        ? throw new ArgumentNullException(nameof(entity))
        : new ProjectModel()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Budget = entity.Budget,
            Avatar = entity.Avatar,

            StatusId = entity.StatusId,
            CustomerId = entity.CustomerId,
        };

    public static ProjectEntity Create(ProjectModel model) =>
        model is null
        ? throw new ArgumentNullException(nameof(model))
        : new ProjectEntity()
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Budget = model.Budget,
            Avatar = model.Avatar,

            StatusId = model.StatusId,
            CustomerId = model.CustomerId,
        };
}
