using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Dtos;
using Service.Interfaces;
using Service.Services;
using System.Text;
using Ui.Asp.Mvc.Extensions;
using Ui.Asp.Mvc.Models;
using Ui.Asp.Mvc.Services;

namespace Ui.Asp.Mvc.Controllers;

[Authorize(Roles = "Administrator")]
public class MembersController(
                    IUserService userService, 
                    IRoleService roleService, 
                    IJobTitleService jobTitleService,
                    ImageManager imageManager,
                    IMailService mailService,
                    LinkGenerationService linkGenerationService,
                    ILogger<MembersController> logger) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly IRoleService _roleService = roleService;
    private readonly IJobTitleService _jobTitleService = jobTitleService;
    private readonly ImageManager _imageManager = imageManager;
    private readonly IMailService _mailService = mailService;
    private readonly LinkGenerationService _linkGenerationService = linkGenerationService;
    private readonly ILogger<MembersController> _logger = logger;

    [Route("/members")]
    public async Task<IActionResult> IndexAsync()
    {
        MembersViewModel viewModel = new()
        {
            Roles = await _roleService.GetAllAsync(),
            Members = await _userService.GetAllAsync(),
            JobTitles = await _jobTitleService.GetAllAsync(),
        };

        ViewBag.Roles = viewModel.Roles
            .Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            });

        ViewBag.JobTitles = viewModel.JobTitles
            .Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Title
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
        {
            #region Send confirm email
            var emailConfirmLink = await _linkGenerationService.CreateEmailConfirmLink(dto.Email);
            var msgBody = new StringBuilder()
                        .Append($"<strong>Admin has registered you on Alpha Project Panel</strong>")
                        .Append($"<p>If you have any questions, contact Admin by email: {User.Identity.Name}</p>")
                        .Append($"<p>Please Confirm your email with this link:</p>")
                        .Append($"{emailConfirmLink}");

            if (!string.IsNullOrEmpty(emailConfirmLink))
            {
                var emailResult = await _mailService.SendEmail(msgBody.ToString(), dto.Email);
                TempData["Message"] = emailResult
                    ? "A confirmation Link has been sent to the registered Email"
                    : "There was a problem sending the confirmation Link to the registered Email";
            }
            #endregion

            return Ok();
        }

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

        _logger.LogWarning("\n############################################\n");
        _logger.LogWarning("There was errors updating");
        _logger.LogWarning("\n############################################\n");

        return Ok("There was errors when updating project");
    }
}
