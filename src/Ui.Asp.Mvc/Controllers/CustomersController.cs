using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Ui.Asp.Mvc.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ui.Asp.Mvc.Controllers;

[Authorize(Roles = "Administrator")]
public class CustomersController(ICustomerService customerService) : Controller
{
    private readonly ICustomerService _customerService = customerService;

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var viewModel = new CustomersViewModel()
        {
            customers = await _customerService.GetAllAsync(),
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(CustomerFormViewModel form)
    {
        if (!ModelState.IsValid || form == null)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new { success = false, errors });
        }

        var result = await _customerService.CreateAsync(form);
        if (result)
            return Ok();

        return BadRequest(new { success = false, ModelState});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(CustomerFormViewModel form)
    {
        if (!ModelState.IsValid || form == null)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new { success = false, errors });
        }

        var result = await _customerService.UpdateAsync(form.Id, form);
        if (result)
            return Ok();

        return BadRequest(new { success = false, ModelState });
    }

    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = _customerService.DeleteAsync(id);

        return RedirectToAction("Index");
    }
}
