using Data.Entities;

namespace Service.Models;
public class CustomerModel
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public IEnumerable<ProjectEntity>? Projects { get; set; }
}
