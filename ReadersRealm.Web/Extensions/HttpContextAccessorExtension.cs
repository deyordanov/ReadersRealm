namespace ReadersRealm.Web.Extensions;


public static class HttpContextAccessorExtension
{
    public static string GetDomain(this IHttpContextAccessor context)
    {
        HttpRequest request = context.HttpContext!.Request;
        return $"{request.Scheme}://{request.Host}";
    }
}