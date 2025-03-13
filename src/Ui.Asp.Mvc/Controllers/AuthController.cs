using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ui.Asp.Mvc.Models;

namespace Ui.Asp.Mvc.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<RoleEntity> _roleManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager, ILogger<AuthController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
    {
        _logger.LogInformation($"{model.FirstName} {model.LastName} - <{model.Email}>");

        if (ModelState.IsValid)
        {
            _logger.LogInformation("Modelstate is valid");

            var result = await _userManager.CreateAsync(new UserEntity()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName, 
                LastName = model.LastName
            }, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("UserManager managed to create User");

                return RedirectToAction("Login");
            }
            else
            {
                _logger.LogInformation("UserManager DID NOT managed to create User");

                return View(model);
            }
        }
        else
        {
            _logger.LogInformation("Modelstate is NOT valid");

            return View(model);
        }


            
    }

    public async Task<IActionResult> LogOutAsync()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }

        return View(model);
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(ProjectsController.Index), "Projects");
        }
    }
}
