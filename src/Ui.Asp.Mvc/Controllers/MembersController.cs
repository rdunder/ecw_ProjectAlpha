using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Dtos;
using Service.Interfaces;
using Ui.Asp.Mvc.Models;
using Ui.Asp.Mvc.Services;

namespace Ui.Asp.Mvc.Controllers;

[Authorize(Roles = "Administrator")]
public class MembersController(IUserService userService, IRoleService roleService, ImageManager imageManager, ILogger<MembersController> logger) : Controller
{
    IUserService _userService = userService;
    IRoleService _roleService = roleService;
    ImageManager _imageManager = imageManager;
    ILogger<MembersController> _logger = logger;

    [Route("/members")]
    [AllowAnonymous]
    public async Task<IActionResult> IndexAsync()
    {
        MembersViewModel viewModel = new()
        {
            Roles = await _roleService.GetAllAsync(),
            Members = await _userService.GetAllAsync()
        };

        ViewBag.Roles = viewModel.Roles
            .Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            });

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAsync(MemberFormViewModel form)
    {        

        if (!ModelState.IsValid || form == null)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new { success = false, errors });
        }

        if (form.File != null)
        {
            form.Avatar = await _imageManager.SaveImage(form.File, nameof(MembersController));
        }

        UserDto dto = form;
        dto.Password = PasswordManager.CreatePassword();

        var result = await _userService.CreateAsync(dto);

        if (result)
            return Ok();

        return Ok();
    }
    
    public async Task<IActionResult> DeleteAsync(Guid id, string avatar)
    {
        if (!string.IsNullOrEmpty(avatar))
            _imageManager.DeleteImage(avatar, nameof(MembersController));
        
        await _userService.DeleteAsync(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> EditAsync(MemberFormViewModel form)
    {
        if (!ModelState.IsValid || form == null)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new { success = false, errors });
        }

        if (form.File != null)
        {
            if (form.Avatar != null)
                _imageManager.DeleteImage(form.Avatar, nameof(MembersController));

            form.Avatar = await _imageManager.SaveImage(form.File, nameof(MembersController));
        }

        var result = await _userService.UpdateAsync(form.Id, form);
        if (result) return Ok();

        _logger.LogInformation("\n############################################\n");
        _logger.LogInformation("There was errors updating");
        _logger.LogInformation("\n############################################\n");

        return Ok("There was errors when updating project");
    }
}
