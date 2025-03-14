using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ui.Asp.Mvc.Controllers;

[Authorize]
public class ProjectsController : Controller
{    
    public IActionResult Index()
    {
        
        return View();
    }
}
