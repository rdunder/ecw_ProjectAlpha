

using Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Service.Models;

public class ProjectModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Budget { get; set; }
    public string? Avatar { get; set; }


    public Guid StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;


    public Guid CustomerId { get; set; }
    public CustomerEntity Customer { get; set; } = null!;


    public IEnumerable<UserEntity>? Users { get; set; }
}
