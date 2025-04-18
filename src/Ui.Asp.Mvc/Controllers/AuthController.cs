using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Service.Dtos;
using Service.Interfaces;
using System.Security.Claims;
using System.Text;
using Ui.Asp.Mvc.Models.Auth;
using Ui.Asp.Mvc.Services;
using Ui.Asp.Mvc.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Ui.Asp.Mvc.Controllers;

public class AuthController(
    SignInManager<UserEntity> signInManager,
    UserManager<UserEntity> userManager,
    RoleManager<RoleEntity> roleManager,
    ILogger<AuthController> logger,
    InitService initService,
    IUserService userService,
    IMailService mailService,
    LinkGenerationService linkGenerationService,
    INotificationService notificationService,
    IHubContext<NotificationHub> notificationsHub) : Controller
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<RoleEntity> _roleManager = roleManager;
    private readonly ILogger<AuthController> _logger = logger;
    private readonly InitService _initService = initService;
    private readonly IUserService _userService = userService;
    private readonly IMailService _mailService = mailService;
    private readonly LinkGenerationService _linkGenerationService = linkGenerationService;

    private readonly INotificationService _notificationService = notificationService;
    private readonly IHubContext<NotificationHub> _notificationsHub = notificationsHub;

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

            #region Send confirm email
            var emailConfirmLink = await _linkGenerationService.CreateEmailConfirmLink(viewModel.Email);
            var msgBody = new StringBuilder()
                        .Append("<strong>If you did not request this information, please do NOT click the link</strong>")
                        .Append($"<p>Please Confirm your email with this link:</p>")
                        .Append($"{emailConfirmLink}");

            if (!string.IsNullOrEmpty(emailConfirmLink))
            {
                var emailResult = await _mailService.SendEmail(msgBody.ToString(), viewModel.Email);
                TempData["Message"] = emailResult 
                    ? "A confirmation Link has been sent to the registered Email"
                    : "There was a problem sending the confirmation Link to the registered Email";
            }
            #endregion

            #region Send notification to admins
            var notification = new NotificationDto
            {
                Message = $"{viewModel.FirstName} {viewModel.LastName} Added",
                Icon = "/images/NotificationIcons/MemberNotification.png",
                TargetGroup = NotificationTargetGroup.Admins,
                Type = NotificationType.User
            };

            await _notificationService.CreateNotificationAsync(notification);
            await _notificationsHub.Clients.Group("Administrator").SendAsync("ReceiveNotification", notification);
            #endregion


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

        //  If the app is started for the first time, the initservice will run:
        #region

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

            UserEntity entity = await _userService.CreateExternalAsync(dto, externalLoginInfo);
            if (entity != null)
            {                
                //await _signInManager.SignInAsync(entity, isPersistent: false);
                await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, isPersistent: false, bypassTwoFactor: true);

                #region Send confirm email 
                try
                {
                    var emailConfirmLink = await _linkGenerationService.CreateEmailConfirmLink(entity.Email);
                    var msgBody = new StringBuilder()
                        .Append("<strong>If you did not request this information, please do NOT click the link</strong>")
                        .Append($"<p>Please Confirm your email with this link:</p>")
                        .Append($"{emailConfirmLink}");

                    if (!string.IsNullOrEmpty(emailConfirmLink))
                    {
                        var emailResult = await _mailService.SendEmail(msgBody.ToString(), entity.Email);
                        TempData["Message"] = emailResult
                            ? "A confirmation Link has been sent to the registered Email"
                            : "There was a problem sending the confirmation Link to the registered Email";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Email with confirmEmail Link was not sent: {ex.Message}");
                }                
                #endregion

                #region Send notification to admins
                var notification = new NotificationDto
                {
                    Message = $"{entity.FirstName} {entity.LastName} Added",
                    Icon = "/images/NotificationIcons/MemberNotification.png",
                    TargetGroup = NotificationTargetGroup.Admins,
                    Type = NotificationType.User
                };

                await _notificationService.CreateNotificationAsync(notification);
                await _notificationsHub.Clients.Group("Administrator").SendAsync("ReceiveNotification", notification);
                #endregion

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
}
