using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos;
using Service.Interfaces;

namespace Ui.Asp.Mvc.Controllers;

[Authorize]
public class ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger, IWebHostEnvironment env) : Controller
{    
    private readonly IProjectService _projectService = projectService;
    private readonly ILogger<ProjectsController> _logger = logger;
    private readonly IWebHostEnvironment _env = env;

    public async Task<IActionResult> IndexAsync()
    {
        var projects = await _projectService.GetAllAsync();   
        return View(projects);
    }

    public async Task<IActionResult> AddAsync()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(ProjectDto dto)
    {
        if (!ModelState.IsValid)
            return PartialView("_ProjectForm");

        if (dto.File == null)
            return RedirectToAction("Index");

        var uploadFolder = Path.Combine(_env.WebRootPath, "images/project_avatars");
        Directory.CreateDirectory(uploadFolder);

        var filePath = Path.Combine(uploadFolder, $"{dto.Name.Replace(" ", "_")}_Avatar_{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await dto.File.CopyToAsync(stream);
        }

        _logger.LogInformation($"####\n{filePath}\n{dto.File.FileName}\n####");
        return RedirectToAction("Index");
    }
}
