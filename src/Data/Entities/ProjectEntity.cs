

using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectEntity
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    [Required]
    public decimal Budget { get; set; }

    public string? Avatar { get; set; }


    public int StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;


    public int CustomerId { get; set; }
    public CustomerEntity Customer { get; set; } = null!;

    public ICollection<UserEntity> Users { get; set; } = [];

}
