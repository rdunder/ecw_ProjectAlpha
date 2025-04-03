using Service.Models;

namespace Ui.Asp.Mvc.Models;

public class AddMemberToProjectFormModel
{
    public List<Guid> MemberIds { get; set; } = [];
    public ProjectModel? Project { get; set; }
    public Guid ProjectId { get; set; }
}
