﻿using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Service.Interfaces;
using System.Text;
using System.Text.Json;
using Data.Enums;
using Microsoft.AspNetCore.SignalR;
using Service.Dtos;
using Ui.Asp.Mvc.Hubs;
using Ui.Asp.Mvc.Models;
using Ui.Asp.Mvc.Models.Account;
using Ui.Asp.Mvc.Services;

namespace Ui.Asp.Mvc.Controllers;

[Authorize]
public class AccountController(
    IUserService userService, 
    UserManager<UserEntity> userManager,
    IMailService mailService,
    LinkGenerationService linkGenerationService,
    ImageManager imageManager,
    INotificationService notificationService,
    IHubContext<NotificationHub> notificationHub,
    ILogger<AccountController> logger) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly IMailService _mailService = mailService;
    private readonly LinkGenerationService _linkGenerationService = linkGenerationService;
    private readonly ImageManager _imageManager = imageManager;
    private readonly INotificationService _notificationService = notificationService;
    private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;
    private readonly ILogger<AccountController> _logger = logger;

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
                Address = user.Address?.Address,
                PostalCode = user.Address?.PostalCode,
                City = user.Address?.City,
                RoleName = user.RoleName!,
                JobTitleId = user.JobTitleId,
            },

            ChangePasswordForm = new()
            {
                Id = user.Id
            }
            
        };

        ViewBag.ErrorMessage = "";

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
        
        if (form.File != null)
        {
            _imageManager.DeleteImage(form.Avatar, nameof(MembersController));
            form.Avatar = await _imageManager.SaveImage(form.File, nameof(MembersController));
        }

        var result = await _userService.UpdateAsync(form.Id, form);

        if (result)
            return RedirectToAction("Index");

        ViewBag.ErrorMessage = "Profile information failed to update";
        return View(viewModel);
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

    [HttpGet]
    public async Task<IActionResult> GetPersonalInformation()
    {
        var userInfo = await _userService.GetPersonalData(User);
        return File(userInfo, "text/json", "PersonalData.json");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAccount(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        var result = await _userService.DeleteAsync(id);
        if (result)
        {
            var notification = new NotificationDto()
            {
                Message = $"{user.FirstName} {user.LastName} has been deleted",
                Icon = "/images/NotificationIcons/MemberNotification.png",
                TargetGroup = NotificationTargetGroup.Admins,
                Type = NotificationType.User,
                TypeName = nameof(NotificationType.User)
            };

            await _notificationService.CreateNotificationAsync(notification);
            await _notificationHub.Clients.Group("Administrator").SendAsync("ReceiveNotification", notification);
        }
        
        return RedirectToAction("LogOut", "Auth");
    }


    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(Guid userId, string? code)
    {
        if (userId == Guid.Empty || code == null) return RedirectToAction("Login", "Auth");

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
    public IActionResult RequestPasswordReset()
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

        var passwordResetLink = await _linkGenerationService.CreatePasswordResetLink(form.Email);
        if (!string.IsNullOrEmpty(passwordResetLink))
        {
            //var msgBody = $"Click this link to reset your password: {passwordResetLink}";
            var msgBody = new StringBuilder()
                        .Append("<strong>If you did not request this information, please do NOT click the link</strong>")
                        .Append($"<p>Click the link to reset your password:</p>")
                        .Append($"{passwordResetLink}");

            var emailResult = await _mailService.SendEmail(msgBody.ToString(), form.Email);

            ViewBag.EmailSent = emailResult;
        }
        
        return View();
    }

    [AllowAnonymous]
    public IActionResult ResetPassword(Guid userId, string code)
    {
        var form = new PasswordResetForm
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
