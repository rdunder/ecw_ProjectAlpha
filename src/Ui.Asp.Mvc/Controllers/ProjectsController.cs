using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ui.Asp.Mvc.Controllers;

public class ProjectsController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        
        return View();
    }
}
