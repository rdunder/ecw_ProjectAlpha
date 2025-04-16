using Azure.Core;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;

namespace Ui.Asp.Mvc.Services;

public class LinkGenerationService(
    UserManager<UserEntity> userManager,
    LinkGenerator linkGenerator,
    IHttpContextAccessor httpContextAccessor)
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly LinkGenerator _linkGenerator = linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


    public async Task<string> CreateEmailConfirmLink(string email)
    {
        var entity = await _userManager.FindByEmailAsync(email);
        if (entity == null) return string.Empty;

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(entity);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var httpContext = _httpContextAccessor.HttpContext;
        var confirmationLink = _linkGenerator.GetUriByAction(
            httpContext,
            action: "ConfirmEmail",
            controller: "Account",
            values: new { userId = entity.Id, code },
            scheme: httpContext.Request.Scheme);

        return string.IsNullOrEmpty(confirmationLink) ? string.Empty : confirmationLink;
    }

    public async Task<string> CreatePasswordResetLink(string email)
    {
        var entity = await _userManager.FindByEmailAsync(email);
        if (entity == null) return string.Empty;

        var code = await _userManager.GeneratePasswordResetTokenAsync(entity);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var httpContext = _httpContextAccessor.HttpContext;

        var callbackUrl = _linkGenerator.GetUriByAction(
            httpContext,
            action: "ResetPassword",
            controller: "Account",
            values: new { userId = entity.Id, code },
            scheme: httpContext.Request.Scheme);


        return string.IsNullOrEmpty(callbackUrl) ? string.Empty : callbackUrl;
    }
}
