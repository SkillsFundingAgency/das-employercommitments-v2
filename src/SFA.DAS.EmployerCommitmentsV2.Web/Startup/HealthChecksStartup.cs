using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.HealthChecks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class HealthChecksStartup
    {
        public static IServiceCollection AddDasHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<CommitmentsApiHealthCheck>("Commitments API Health Check");
            
            return services;
        }
        
        public static IApplicationBuilder UseDasHealthChecks(this IApplicationBuilder app)
        {
            return app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = (c, r) => c.Response.WriteJsonAsync(new
                {
                    r.Status,
                    r.TotalDuration,
                    Results = r.Entries.ToDictionary(
                        e => e.Key,
                        e => new
                        {
                            e.Value.Status,
                            e.Value.Duration,
                            e.Value.Description,
                            e.Value.Data
                        })
                })
            });
        }
    }
}