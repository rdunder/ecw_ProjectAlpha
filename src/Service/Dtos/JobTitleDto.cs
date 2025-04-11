

using Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Service.Dtos;

public class JobTitleDto
{
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;
}
