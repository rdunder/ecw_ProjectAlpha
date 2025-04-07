using Microsoft.AspNetCore.Mvc;
using Ui.Asp.Mvc.Models;

namespace Ui.Asp.Mvc.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CustomerFormViewModel form)
        {

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(CustomerFormViewModel form)
        {

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteAsync(Guid id)
        {

            return RedirectToAction("Index");
        }
    }
}
