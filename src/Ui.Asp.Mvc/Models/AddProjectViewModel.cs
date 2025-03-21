using Service.Dtos;

namespace Ui.Asp.Mvc.Models;

public class AddProjectViewModel
{
    public IFormFile File { get; set; } = null!;

    public ProjectDto projectDto { get; set; } = new ProjectDto();
}
