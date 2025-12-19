using System.Security.Claims;

namespace ValeraWeb.Security;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var id =
            user.FindFirstValue("UserId")
            ?? user.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.Parse(id ?? throw new InvalidOperationException("UserId claim not found"));
    }

    public static bool IsAdmin(this ClaimsPrincipal user)
        => string.Equals(user.FindFirstValue("role") ?? user.FindFirstValue(ClaimTypes.Role), "Admin",
            StringComparison.OrdinalIgnoreCase);
}