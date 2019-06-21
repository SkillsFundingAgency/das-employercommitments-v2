using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Mvc.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class MvcStartup
    {
        public static IServiceCollection AddDasMvc(this IServiceCollection services)
        {
            services.AddMvc(o=>o.AddAuthorization())
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services;
        }
    }
}