using Service.Models;
using System.ComponentModel.DataAnnotations;
using Ui.Asp.Mvc.Extensions;

namespace Ui.Asp.Mvc.Models.Account;

public class AccountViewModel
{
    public UserModel CurrentUser { get; set; } = new();
    public MemberFormViewModel MemberForm { get; set; } = new();

    public ChangePasswordFormModel? ChangePasswordForm { get; set; } = new();
}
