using System.Security.Claims;

namespace Ui.Asp.Mvc.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetExternalLoginProvider(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.AuthenticationMethod);
    }

    public static string? GetFullName(this ClaimsPrincipal user)
    {
        var firstName = user?.FindFirstValue(ClaimTypes.GivenName) ??
                    user?.FindFirstValue("given_name") ??
                    user?.FindFirstValue("FirstName") ??
                    user?.FindFirstValue("name.first") ??
                    string.Empty;

        var lastName = user?.FindFirstValue(ClaimTypes.Surname) ??
                       user?.FindFirstValue("family_name") ??
                       user?.FindFirstValue("LastName") ??
                       user?.FindFirstValue("name.last") ??
                       string.Empty;

        return $"{firstName} {lastName}".Trim();
    }
}
