using Data.Entities;
using Service.Models;

namespace Ui.Asp.Mvc.Models;

public class MembersViewModel
{
    public IEnumerable<RoleModel> Roles { get; set; } = [];
    public IEnumerable<UserModel> Members { get; set; } = [];

    public MemberFormViewModel form { get; set; } = new();
}
