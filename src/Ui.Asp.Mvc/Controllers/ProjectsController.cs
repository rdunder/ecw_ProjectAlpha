using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Dtos;
using Service.Interfaces;
using Service.Models;
using Ui.Asp.Mvc.Models;
using Ui.Asp.Mvc.Services;

namespace Ui.Asp.Mvc.Controllers;

[Authorize]
public class ProjectsController(IProjectService projectService, ICustomerService customerService, IStatusService statusService, ImageManager imageManager, ILogger<ProjectsController> logger) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly ICustomerService _customerService = customerService;
    private readonly IStatusService _statusService = statusService;
    private readonly ImageManager _imageManager = imageManager;
    private readonly ILogger<ProjectsController> _logger = logger;

    public async Task<IActionResult> IndexAsync()
    {

        ProjectsViewModel viewModel = new()
        {
            Projects = await _projectService.GetAllAsync(),
            Customers = await _customerService.GetAllAsync(),
            Statuses = await _statusService.GetAllAsync()
        };        


        ViewBag.Customers = viewModel.Customers
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(), Text = c.CustomerName
            }).ToList();

        ViewBag.Statuses = viewModel.Statuses
            .Select(s => new SelectListItem
            {
                Value= s.Id.ToString(), Text = s.StatusName
            }).ToList();

        ViewBag.CountAll = viewModel.Projects.Count();
        ViewBag.CountPending = viewModel.Projects.Count(p => p.Status.StatusName == "Pending");
        ViewBag.CountStarted = viewModel.Projects.Count(p => p.Status.StatusName == "Active");
        ViewBag.CountCompleted = viewModel.Projects.Count(p => p.Status.StatusName == "Closed");



        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> AddAsync(ProjectFormViewModel form)
    {
        if (!ModelState.IsValid || form == null)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new {success = false, errors});
        }

        form.StatusId = await GetStatus(form);

        if (form.File != null)
        {
            form.Avatar = await _imageManager.SaveImage(form.File, nameof(ProjectsController));
        }

        var result = await _projectService.CreateAsync(form);
        if (result)
            return Ok();

        return Ok();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> EditAsync(ProjectFormViewModel form)
    {
        if (!ModelState.IsValid || form == null)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new {success = false, errors});
        }

        form.StatusId = await GetStatus(form);

        if (form.File != null)
        {
            _imageManager.DeleteImage(form.Avatar, nameof(ProjectsController));
            form.Avatar = await _imageManager.SaveImage(form.File, nameof(ProjectsController));
        }

        var result = await _projectService.UpdateAsync(form.Id, form);
        if (result) return Ok();

        _logger.LogInformation("\n############################################\n");
        _logger.LogInformation("There was errors updating");
        _logger.LogInformation("\n############################################\n");

        return Ok("There was errors when updating project");
    }

    public async Task<IActionResult> DeleteAsync(Guid id, string avatar)
    {
        if (!string.IsNullOrEmpty(avatar))
            _imageManager.DeleteImage(avatar, nameof(ProjectsController));
        
        await _projectService.DeleteAsync(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> StartProject(Guid id)
    {
        var project = await _projectService.StartProjectAsync(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> CloseProject(Guid id)
    {
        var project = await _projectService.CloseProjectAsync(id);
        return RedirectToAction("Index");
    }


    private async Task<Guid> GetStatus(ProjectFormViewModel form)
    {
        var statuses = await _statusService.GetAllAsync();
        var today = DateOnly.FromDateTime(DateTime.Now);
        var startDate = form.StartDate;
        //var startDateTimeDiff = form.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now;

        if (startDate > today)
            return statuses.FirstOrDefault(s => s.StatusName == "Pending").Id;
        else
            return statuses.FirstOrDefault(s => s.StatusName == "Active").Id;
    }


}

