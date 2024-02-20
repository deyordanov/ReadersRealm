namespace ReadersRealm.Web.Infrastructure.Extensions;

using Microsoft.AspNetCore.Http;

public static class HttpContextAccessorExtensions
{
    public static string GetDomain(this IHttpContextAccessor context)
    {
        HttpRequest request = context.HttpContext!.Request;
        return $"{request.Scheme}://{request.Host}";
    }
}