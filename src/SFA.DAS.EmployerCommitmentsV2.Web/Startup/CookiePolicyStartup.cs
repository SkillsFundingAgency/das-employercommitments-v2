using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class CookiePolicyStartup
    {
        public static IServiceCollection AddDasCookiePolicy(this IServiceCollection services)
        {
            return services.Configure<CookiePolicyOptions>(o =>
            {
                o.CheckConsentNeeded = c => true;
                o.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }
    }
}