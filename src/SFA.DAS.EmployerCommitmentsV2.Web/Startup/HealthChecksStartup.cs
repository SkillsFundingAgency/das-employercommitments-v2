using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Web.HealthChecks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class HealthChecksStartup
    {
        public static IServiceCollection AddDasHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks();
            
            return services;
        }
        
        public static IApplicationBuilder UseDasHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
            });

            return app;
        }
    }
}