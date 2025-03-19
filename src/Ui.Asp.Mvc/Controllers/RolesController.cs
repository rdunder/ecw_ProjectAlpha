using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Ui.Asp.Mvc.Controllers;

[Authorize(Roles = "Administrator")]
public class RolesController(IRoleService roleService) : Controller
{
    [Route("/roles")]
    public async Task<IActionResult> IndexAsync()
    {
        var roles = await roleService.GetAllAsync();
        return View(roles);
    }

    // GET: RolesController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: RolesController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: RolesController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: RolesController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: RolesController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: RolesController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: RolesController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
