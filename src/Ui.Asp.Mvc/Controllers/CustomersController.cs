using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Ui.Asp.Mvc.Models;

namespace Ui.Asp.Mvc.Controllers;

public class CustomersController(ICustomerService customerService) : Controller
{
    private readonly ICustomerService _customerService = customerService;

    public async Task<IActionResult> Index()
    {
        var viewModel = new CustomersViewModel()
        {
            customers = await _customerService.GetAllAsync(),
        };

        return View(viewModel);
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
