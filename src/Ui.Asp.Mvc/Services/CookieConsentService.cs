using Microsoft.AspNetCore.Http.Features;
using System.Text.Json;
using Ui.Asp.Mvc.Models;

namespace Ui.Asp.Mvc.Services;

public class CookieConsentService(IHttpContextAccessor httpContextAccesor)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccesor;
    private readonly string _consentCookieName = "CookieConsentPrefs";

    public CookieConsentPreferences GetConsent()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null && context.Request.Cookies.TryGetValue(_consentCookieName, out var cookie))
        {
            try
            {
                return JsonSerializer.Deserialize<CookieConsentPreferences>(cookie)!;
            }
            catch
            {
                return new CookieConsentPreferences();
            }
        }
        return new CookieConsentPreferences();
    }

    public void SetConsent(CookieConsentPreferences prefs)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return;

        var cookieOpt = new CookieOptions()
        {
            Expires = DateTime.Now.AddMonths(3),
            Path = "/",
            SameSite = SameSiteMode.Lax,
            HttpOnly = false
        };

        var prefsJson = JsonSerializer.Serialize(prefs);
        context?.Response.Cookies.Append(_consentCookieName, prefsJson, cookieOpt);
    }

    public bool IsConsentGiven(string category)
    {
        var context = _httpContextAccessor.HttpContext;
        var consentFeature = context!.Features.Get<ITrackingConsentFeature>();
        if (consentFeature != null && category == "general")
                return consentFeature.CanTrack ? true : false;

        var prefs = GetConsent();

        return category.ToLower() switch
        {
            "essential"     => prefs.Essential,
            "functional"    => prefs.Functional,
            "analytics"     => prefs.Analytics,
            "marketing"     => prefs.Marketing,
            _ => false
        };
    }
}
