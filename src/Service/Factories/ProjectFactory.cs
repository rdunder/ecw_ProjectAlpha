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
            DateCreated = dto.DateCreated,
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
            DateCreated = entity.DateCreated,
            Budget = entity.Budget,
            Avatar = entity.Avatar,

            Status = StatusFactory.Create(entity.Status),
            Customer = CustomerFactory.Create(entity.Customer),
            Users = entity.Users.Select(u => UserFactory.Create(u))
        };

    public static void Map(ProjectDto dto, ProjectEntity entity)
    {
        entity.Id = dto.Id;
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.StartDate = dto.StartDate;
        entity.EndDate = dto.EndDate;
        entity.DateCreated = dto.DateCreated;
        entity.Budget = dto.Budget;
        entity.Avatar = dto.Avatar;
        entity.StatusId = dto.StatusId;
        entity.CustomerId = dto.CustomerId;
    }
}
