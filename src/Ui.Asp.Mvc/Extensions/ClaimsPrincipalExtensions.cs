using System.Security.Claims;

namespace Ui.Asp.Mvc.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetFirstName(this ClaimsPrincipal user)
       => user?.FindFirst("FirstName")?.Value;

    public static string? GetLastName(this ClaimsPrincipal user)
        => user?.FindFirst("LastName")?.Value;
}
