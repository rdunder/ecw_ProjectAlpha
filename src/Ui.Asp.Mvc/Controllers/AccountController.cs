using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos;
using Service.Interfaces;
using Service.Models;
using Ui.Asp.Mvc.Models;
using Ui.Asp.Mvc.Models.Account;

namespace Ui.Asp.Mvc.Controllers;

[Authorize]
public class AccountController(IUserService userService) : Controller
{
    private readonly IUserService _userService = userService;

    public async Task<IActionResult> Index(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);

        var viewModel = new AccountViewModel
        {
            CurrentUser = user,

            MemberForm = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate,
                Avatar = user.Avatar,
                Address = user.Address.Address,
                PostalCode = user.Address.PostalCode,
                City = user.Address.City,
                RoleName = user.RoleName!
            },

            ChangePasswordForm = new()
            {
                Id = user.Id
            }
            
        };
        ViewBag.ErrorMessage = "hello hello";

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(MemberFormViewModel form)
    {
        var viewModel = new AccountViewModel
        {
            MemberForm = form,
            ChangePasswordForm = new() { Id = form.Id }            
        };

        if (!ModelState.IsValid)
            return View(viewModel);        

        var result = await _userService.UpdateAsync(form.Id, form);

        if (result)
            return RedirectToAction("Index");

        ViewBag.ErrorMessage = "Profile information failed to update";
        return View(form);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePassword(ChangePasswordFormModel form)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", new { id = form.Id });

        var result = await _userService.UpdatePassword(form.Id, form.OldPassword, form.NewPassword);
        
        if (result)
        {
            ViewBag.ErrorMessage = "Password Successfully updated";
            return RedirectToAction("Index", new { id = form.Id });

        }


        ViewBag.ErrorMessage = "Failed to update password";
        return RedirectToAction("Index", new { id = form.Id });
    }

    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(Guid userId, string code)
    {
        if (userId == Guid.Empty || code == null) return RedirectToAction("Auth", "Login");

        // var result = await _userService.ConfirmEmail(userId, code)
        var result = true;

        
        if (result)
        {
            ViewBag.StatusMessage = "Thank you for confirming your email.";
            ViewBag.UserId = userId.ToString();
            ViewBag.Code = code;
        }
        else
        {
            ViewBag.StatusMessage = "Error confirming your email. contact Admin!";
        }

        return View();
    }
}
