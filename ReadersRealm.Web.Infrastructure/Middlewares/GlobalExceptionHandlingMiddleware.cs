namespace ReadersRealm.Web.Infrastructure.Middlewares;

using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using static Common.Constants.Constants.ErrorConstants;

public class GlobalExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);

        }
        catch (BaseNotFoundException _)
        {
            context.Response.Redirect(NotFound404Path);
        }
        catch (Exception ex)
        {
            context.Response.Redirect(InternalServerError500Path);
        }

        if (context.Response is { HasStarted: false, StatusCode: 404 })
        {
            context.Response.Redirect(NotFound404Path);
        }
    }
}