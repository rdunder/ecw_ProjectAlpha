using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class JobTitleEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public ICollection<UserEntity> Users { get; set; } = [];
}
