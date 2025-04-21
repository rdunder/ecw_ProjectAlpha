namespace Service.Models;
public class ProjectModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly DateCreated { get; set; }
    public decimal Budget { get; set; }
    public string? Avatar { get; set; }

    public StatusModel Status { get; set; } = null!;

    public CustomerModel Customer { get; set; } = null!;

    public IEnumerable<UserModel>? Users { get; set; } = [];
}
