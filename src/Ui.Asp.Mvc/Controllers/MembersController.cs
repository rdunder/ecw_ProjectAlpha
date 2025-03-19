using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models;

namespace Ui.Asp.Mvc.Controllers;

[Authorize(Roles = "Administrator")]
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

    
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _userService.DeleteAsync(id);

        return RedirectToAction("Index");
    }



    public async Task<IActionResult> EditAsync(UserModel user)
    {
        _logger.LogInformation($"####\n\nUser: {user.FirstName} {user.LastName}\n{user.Id}\n{user.BirthDate}\n\n####");

        return RedirectToAction("Index");
    }
}
