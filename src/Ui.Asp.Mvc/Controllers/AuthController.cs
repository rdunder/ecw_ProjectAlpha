using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos;
using Service.Interfaces;
using System.Security.Claims;
using Ui.Asp.Mvc.Models;
using Ui.Asp.Mvc.Services;

namespace Ui.Asp.Mvc.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<RoleEntity> _roleManager;
    private readonly ILogger<AuthController> _logger;
    private readonly InitService _initService;

    private readonly IUserService _userService;

    public AuthController(
        SignInManager<UserEntity> signInManager, 
        UserManager<UserEntity> userManager, 
        RoleManager<RoleEntity> roleManager, 
        ILogger<AuthController> logger,
        InitService initService,
        IUserService userService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _initService = initService;

        _userService = userService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(UserDto dto)
    {
        if (dto.Password == null)
        {
            dto.Password = "Password123!";
            dto.ConfirmPassword = "Password123!";
            ModelState.Clear();
            TryValidateModel(dto);
        }

        if (ModelState.IsValid)
        {
            _logger.LogInformation("Modelstate is valid");
            var result = await _userService.CreateAsync(dto);
            return result ? RedirectToAction("Login") : View(dto);         
        }
        else
        {
            _logger.LogInformation("Modelstate is NOT valid");
            return View(dto);
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
            return RedirectToAction("Index", "Projects");
        }
    }
}
