using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;
using Ui.Asp.Mvc.Models;

namespace Ui.Asp.Mvc.Controllers;

[Authorize(Roles = "Administrator")]
public class JobTitlesController(IJobTitleService jobTitleService) : Controller
{
    private readonly IJobTitleService _jobTitleService = jobTitleService;

    public async Task<IActionResult> IndexAsync()
    {
        var viewModel = new JobTitlesViewModel
        {
            JobTitles = await _jobTitleService.GetAllAsync()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(JobTitlesFormViewModel form)
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

        var result = await _jobTitleService.CreateAsync(form);
        if (result)
            return Ok();

        return BadRequest(new { success = false, ModelState });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(JobTitlesFormViewModel form)
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

        var result = await _jobTitleService.UpdateAsync(form.Id, form);
        if (result)
            return Ok();

        return BadRequest(new { success = false, ModelState });
    }

    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _jobTitleService.DeleteAsync(id);

        return RedirectToAction("Index");
    }
}
