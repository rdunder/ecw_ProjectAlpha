using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ui.Asp.Mvc.Models;
using Ui.Asp.Mvc.Services;

namespace Ui.Asp.Mvc.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CookieConsentController(CookieConsentService consentService) : ControllerBase
{
    private readonly CookieConsentService _consentService = consentService;

    [HttpGet]
    [Route("")]
    public ActionResult<CookieConsentPreferences> GetConsent()
    {
        return _consentService.GetConsent();
    }

    [HttpPost]
    public ActionResult SetConsent(CookieConsentPreferences preferences)
    {
        _consentService.SetConsent(preferences);
        return Ok();
    }

    
    [HttpGet]
    [Route("IsConsentGiven")]
    public ActionResult<bool> IsConsentGiven(string category)
    {
        return _consentService.IsConsentGiven(category);
    }
}
