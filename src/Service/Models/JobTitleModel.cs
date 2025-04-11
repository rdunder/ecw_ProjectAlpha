

using System.ComponentModel.DataAnnotations;

namespace Service.Models;

public class JobTitleModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
}
