

using Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Service.Models;

public class ProjectModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Budget { get; set; }
    public string? Avatar { get; set; }


    public int StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;


    public int CustomerId { get; set; }
    public CustomerEntity Customer { get; set; } = null!;


    public IEnumerable<UserEntity>? Users { get; set; };
}
