using System.Text.Json.Serialization;
using Ui.Asp.Mvc.Models.Enums;

namespace Ui.Asp.Mvc.Models;
public class CookieConsentPreferences
{
    [JsonPropertyName(nameof(CookieConsentCategory.essential))]
    public bool Essential { get; set; } = true;

    [JsonPropertyName(nameof(CookieConsentCategory.functional))]
    public bool Functional { get; set; } = false;

    [JsonPropertyName(nameof(CookieConsentCategory.analytics))]
    public bool Analytics { get; set; } = false;

    [JsonPropertyName(nameof(CookieConsentCategory.marketing))]
    public bool Marketing { get; set; } = false;
}
