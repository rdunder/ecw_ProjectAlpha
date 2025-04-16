using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Service.Dtos;
using Service.Interfaces;
using Service.Models;
using System.Runtime.CompilerServices;
using System.Text;
using Ui.Asp.Mvc.Models;
using Ui.Asp.Mvc.Models.Account;
using Ui.Asp.Mvc.Services;

namespace Ui.Asp.Mvc.Controllers;

[Authorize]
public class AccountController(
    IUserService userService, 
    UserManager<UserEntity> userManager,
    IMailService mailService,
    LinkGenerationService linkGenerationService) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly IMailService _mailService = mailService;
    private readonly LinkGenerationService _linkGenerationService = linkGenerationService;

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
                RoleName = user.RoleName!,
                JobTitleId = user.JobTitleId,
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

        var result = await _userService.ConfirmEmail(userId, code);
        
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


    #region Password Reset
    [AllowAnonymous]
    public async Task<IActionResult> RequestPasswordReset()
    {
        ViewBag.EmailSent = null;
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RequestPasswordReset(PasswordResetRequestForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        var paswordResetLink = await _linkGenerationService.CreatePasswordResetLink(form.Email);
        if (!string.IsNullOrEmpty(paswordResetLink))
        {
            //var msgBody = $"Click this link to reset your password: {paswordResetLink}";
            var msgBody = new StringBuilder()
                        .Append("<strong>If you did not request this information, please do NOT click the link</strong>")
                        .Append($"<p>Click the link to reset your password:</p>")
                        .Append($"{paswordResetLink}");

            var emailResult = await _mailService.SendEmail(msgBody.ToString(), form.Email);

            ViewBag.EmailSent = emailResult ? true : false;
        }
        
        return View();
    }

    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(Guid userId, string code)
    {
        PasswordResetForm form = new PasswordResetForm
        {
            Id = userId,
            Code = code
        };
            
        return View(form);
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(PasswordResetForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        var user = await _userManager.FindByIdAsync(form.Id.ToString());
        if (user == null) return View(form);

        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(form.Code));
        var result = await _userManager.ResetPasswordAsync(user, code, form.Password);

        if (result.Succeeded)
        {
            TempData["Message"] = "Password Reset Successful";
        }
        else
        {
            TempData["Message"] = "Failed to reset password";
        }
            
        return RedirectToAction("Login", "Auth");
    }

    #endregion
}
