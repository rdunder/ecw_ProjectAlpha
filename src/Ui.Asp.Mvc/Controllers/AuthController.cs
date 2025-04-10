using Data.Entities;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using Service.Dtos;
using Service.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using Ui.Asp.Mvc.Models.Auth;
using Ui.Asp.Mvc.Services;

namespace Ui.Asp.Mvc.Controllers;

public class AuthController(
    SignInManager<UserEntity> signInManager,
    UserManager<UserEntity> userManager,
    RoleManager<RoleEntity> roleManager,
    ILogger<AuthController> logger,
    InitService initService,
    IUserService userService,
    MailService mailService) : Controller
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<RoleEntity> _roleManager = roleManager;
    private readonly ILogger<AuthController> _logger = logger;
    private readonly InitService _initService = initService;
    private readonly IUserService _userService = userService;
    private readonly MailService _mailService = mailService;

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAsync(RegisterViewModel viewModel)
    {
        if (viewModel.Password == null)
        {
            string pwd = PasswordManager.CreatePassword();
            viewModel.Password = pwd;
            viewModel.ConfirmPassword = pwd;
            ModelState.Clear();
            TryValidateModel(viewModel);
        }

        if (ModelState.IsValid)
        {
            _logger.LogInformation("Modelstate is valid");
            var result = await _userService.CreateAsync(viewModel);

            var emailConfirmLink = await CreateEmailConfirmLink(viewModel.Email);
            var msgBody = $"Please Confirm your email with this link:\n{emailConfirmLink}";
            if (!string.IsNullOrEmpty(emailConfirmLink))
            {
                var emailResult = await _mailService.SendEmail(msgBody, viewModel.Email);
                TempData["Message"] = emailResult 
                    ? "A confirmation Link has been sent to the registered Email"
                    : "There was a problem sending the confirmation Link to the registered Email";
            }
            return result ? RedirectToAction("Login") : View(viewModel);         
        }
        else
        {
            _logger.LogInformation("Modelstate is NOT valid");
            return View(viewModel);
        }


            
    }

    public async Task<IActionResult> LogOutAsync()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public async Task<IActionResult> LoginAsync()
    {

        //  If the app is started for the first time, this will be added:
        #region
        //  User: admin@domain.com with password: Password123! and add the Administrator role to that user
        //  Roles: Trainee, Fullstack Developer, Frontend Developer, Backend Developer and Administrator

        if (await _roleManager.RoleExistsAsync("Administrator") == false)
        {
            try
            {
                await _initService.InitCreate();
            }
            catch (Exception e)
            {
                _logger.LogInformation("Something went wrong with creating roles and admin user");
                throw new Exception("Something went wrong with creating roles and admin user");
            }
        }

        //________________________________________________________________________________________________
        #endregion

        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Incorect Email or Password entered.");
                return View(viewModel);
            }
        }

        return View(viewModel);
    }

    public IActionResult AdminLogin()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdminLogin(LoginViewModel viewModel, string returnUrl = null)
    {
        if (!ModelState.IsValid) return View(viewModel);

        var user = await _userManager.FindByEmailAsync(viewModel.Email);

        if (user != null)
        {
            var isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");

            if (isAdmin)
            {
                await _signInManager.SignOutAsync();
                var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded) return RedirectToLocal("/");
            }
        }

        ModelState.AddModelError(string.Empty, "You sure you are ADMIN");
        return View(viewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalSignin(string provider, string returnUrl = null!)
    {

        if (string.IsNullOrEmpty(provider))
        {
            ModelState.AddModelError("", "Invalid Provider");
            return View("Login");
        }

        string redirectUrl = Url.Action("ExternalSigninCallback", "Auth", new { returnUrl })!;
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);

    }

    public async Task<IActionResult> ExternalSigninCallback(string returnUrl = null!, string remoteError = null!)
    {
        returnUrl ??= Url.Content("~/");

        if (!string.IsNullOrEmpty(remoteError))
        {
            ModelState.AddModelError("", $"Error from external provider: {remoteError}");
            return View("Login");
        }

        var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();

        if (externalLoginInfo == null)
        {
            return RedirectToAction("Login");
        }
        
        var signinResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, isPersistent: false, bypassTwoFactor: true);
   
        if (signinResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            UserDto dto = new()
            {
                FirstName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty,
                LastName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty,
                Email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email)!,
                UserName = $"{externalLoginInfo.LoginProvider.ToLower()}_{externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email)}",
                Avatar = externalLoginInfo.Principal.FindFirstValue("picture")
            };


            _logger.LogInformation("###############################################################################################");

            foreach (var claim in externalLoginInfo.Principal.Claims)
            {
                _logger.LogInformation($"{claim}");
            }

            _logger.LogInformation("###############################################################################################");


            UserEntity entity = await _userService.CreateExternalAsync(dto, externalLoginInfo);
            if (entity != null)
            {
                await _signInManager.SignInAsync(entity, isPersistent: false);
                return LocalRedirect(returnUrl);
            }


            return View("Login");
        }
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Index", "Projects");
        }
    }

    private async Task<string> CreateEmailConfirmLink(string email)
    {
        var entity = await _userManager.FindByEmailAsync(email);
        if (entity == null) return string.Empty;
        
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(entity);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var confirmationLink = Url.Action(
            "ConfirmEmail", "Account",
            values: new { userId = entity.Id, code },
            protocol: Request.Scheme);

        return string.IsNullOrEmpty(confirmationLink) ? string.Empty : confirmationLink;
        
    }
}
