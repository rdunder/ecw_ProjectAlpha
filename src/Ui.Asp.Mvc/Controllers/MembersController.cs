using Microsoft.AspNetCore.Mvc;

namespace Ui.Asp.Mvc.Controllers
{
    public class MembersController : Controller
    {
        [Route("/members")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
