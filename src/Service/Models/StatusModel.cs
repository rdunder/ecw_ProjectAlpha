using Data.Entities;

namespace Service.Models;
public class StatusModel
{
    public Guid Id { get; set; }

    public string StatusName { get; set; } = null!;

    public IEnumerable<ProjectEntity>? Projects { get; set; }
}
