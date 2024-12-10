using Microsoft.AspNetCore.Http;
using SFA.DAS.EmployerCommitmentsV2.Exceptions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Middleware;

public class MissingAddApprenticeshipCacheKeyMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (MissingAddApprenticeshipCacheKeyException)
        {
            var redirectUrl = $"/error/404";
            context.Response.Redirect(redirectUrl);
        }
    }
}