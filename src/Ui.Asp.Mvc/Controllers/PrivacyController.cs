using Microsoft.AspNetCore.Mvc;

namespace Ui.Asp.Mvc.Controllers;

public class PrivacyController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
