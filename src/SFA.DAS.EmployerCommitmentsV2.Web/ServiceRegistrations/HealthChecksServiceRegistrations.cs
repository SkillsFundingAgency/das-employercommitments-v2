using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.HealthChecks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class HealthChecksServiceRegistrations
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
            ResponseWriter = (context, healthReport) => context.Response.WriteJsonAsync(new
            {
                healthReport.Status,
                healthReport.TotalDuration,
                Results = healthReport.Entries.ToDictionary(
                    keyValuePair => keyValuePair.Key,
                    keyValuePair => new
                    {
                        keyValuePair.Value.Status,
                        keyValuePair.Value.Duration,
                        keyValuePair.Value.Description,
                        keyValuePair.Value.Data
                    })
            })
        });
    }
}