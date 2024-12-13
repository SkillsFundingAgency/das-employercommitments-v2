using Microsoft.AspNetCore.Http;
using SFA.DAS.EmployerCommitmentsV2.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Middleware;

public class CacheItemNotFoundMiddleware(RequestDelegate next, ILogger<CacheItemNotFoundMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CacheItemNotFoundException<AddApprenticeshipCacheModel> ex)
        {
            logger.LogError(ex, "CacheItemNotFoundException: {message}", ex.Message);
            var redirectUrl = $"/error/404";
            context.Response.Redirect(redirectUrl);
        }
    }
}