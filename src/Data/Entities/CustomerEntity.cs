using System.ComponentModel.DataAnnotations;

namespace Data.Entities;
public class CustomerEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string CustomerName { get; set; } = null!;

    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}
