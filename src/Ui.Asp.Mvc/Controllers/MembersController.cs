using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ui.Asp.Mvc.Controllers;

[Authorize(Roles = "admin")]
public class MembersController : Controller
{

    [Route("/members")]
    public IActionResult Index()
    {      

        return View();
    }
}
