using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Dtos;
using Service.Interfaces;
using Service.Models;
using Ui.Asp.Mvc.Models;

namespace Ui.Asp.Mvc.Controllers;

[Authorize]
public class ProjectsController(
    IProjectService projectService,
    ICustomerService customerService,
    IStatusService statusService,
    ILogger<ProjectsController> logger, 
    IWebHostEnvironment env) : Controller
{    
    private readonly IProjectService _projectService = projectService;
    private readonly ICustomerService _customerService = customerService;
    private readonly IStatusService _statusService = statusService;
    private readonly ILogger<ProjectsController> _logger = logger;
    private readonly IWebHostEnvironment _env = env;

    public async Task<IActionResult> IndexAsync()
    {
        
        ProjectsViewModel viewModel = new()
        {
            Projects = await _projectService.GetAllAsync(),
            Customers = await _customerService.GetAllAsync(),
            Statuses = await _statusService.GetAllAsync(),
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
    public async Task<IActionResult> AddAsync(ProjectDto dto)
    {
        if (!ModelState.IsValid || dto == null)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new {success = false, errors});
        }

        if (dto.File != null)
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, "images/project_avatars");
            Directory.CreateDirectory(uploadFolder);

            var newFileName = $"{dto.Name.Replace(" ", "_")}_Avatar_{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
            var filePath = Path.Combine(uploadFolder, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            dto.Avatar = newFileName;
        }

        var result = await _projectService.CreateAsync(dto);
        if (result)
            return Ok();

        return Ok();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> EditAsync(ProjectDto dto)
    {
        if (!ModelState.IsValid || dto == null)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new {success = false, errors});
        }

        if (dto.File != null)
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, "images/project_avatars");
            Directory.CreateDirectory(uploadFolder);

            var newFileName = $"{dto.Name.Replace(" ", "_")}_Avatar_{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
            var filePath = Path.Combine(uploadFolder, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            dto.Avatar = newFileName;
        }

        var result = await _projectService.UpdateAsync(dto.Id, dto);
        if (result) return Ok();

        _logger.LogInformation("\n############################################\n");
        _logger.LogInformation("There was errors updating");
        _logger.LogInformation("\n############################################\n");

        return Ok("There was errors when updating project");
    }

    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _projectService.DeleteAsync(id);
        return RedirectToAction("Index");
    }


    


}

