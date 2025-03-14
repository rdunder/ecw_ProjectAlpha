using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models;

namespace Ui.Asp.Mvc.Controllers;

[Authorize(Roles = "admin")]
public class MembersController(IUserService userService, ILogger<MembersController> logger) : Controller
{
    IUserService _userService = userService;
    ILogger<MembersController> _logger = logger;

    [Route("/members")]
    public async Task<IActionResult> IndexAsync()
    {
        var users = await _userService.GetAllAsync();

        return View(users);
    }

    
    public async Task<IActionResult> DeleteAsync(string email)
    {
        await _userService.DeleteAsync(email);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> EditAsync(User user)
    {
        _logger.LogInformation($"User: {user.FirstName} {user.LastName}");

        return View(user);
    }
}
