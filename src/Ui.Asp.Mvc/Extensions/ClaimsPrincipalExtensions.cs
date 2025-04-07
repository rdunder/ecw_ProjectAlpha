using System.Security.Claims;

namespace Ui.Asp.Mvc.Extensions;

public static class ClaimsPrincipalExtensions
{
    //  Tried to add provider as claim but i failed
    public static string? GetExternalLoginProvider(this ClaimsPrincipal principal)
    {
        var providerClaim = principal.FindFirst("LoginProvider");
        return providerClaim?.Value;
    }

    //  added fullname as a claim, but never used it
    public static string? GetFullName(this ClaimsPrincipal user)
       => $"{user?.FindFirst("FirstName")?.Value} {user?.FindFirst("LastName")?.Value}";
}
