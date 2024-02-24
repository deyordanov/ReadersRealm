namespace ReadersRealm.Web.Infrastructure.Extensions;

using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}