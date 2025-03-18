

using Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Service.Models;

public class StatusModel
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public IEnumerable<ProjectEntity>? Projects { get; set; }
}
