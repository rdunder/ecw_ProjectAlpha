using System.Diagnostics;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ui.Asp.Mvc.Models;

namespace Ui.Asp.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<RoleEntity> _roleManager;

    public HomeController(ILogger<HomeController> logger, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> IndexAsync()
    {
        //var res = await _userManager.CreateAsync(new UserEntity()
        //{
        //    Email = "something@domain.com",
        //    UserName = "something@domain.com"
        //}, "Password123!");

        //var res = await _roleManager.CreateAsync(new RoleEntity()
        //{
        //    Name = "Admin"
        //});

        //var user = await _userManager.FindByEmailAsync("something@domain.com");
        //var res = await _userManager.AddToRoleAsync(user, "Admin");

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
