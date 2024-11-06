using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

public static class AddOpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetryRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        
        if (!string.IsNullOrEmpty(connectionString))
        {
            services
                .AddOpenTelemetry()
                .UseAzureMonitor(options => options.ConnectionString = connectionString);
        }
        else
        {
            throw new NullReferenceException(nameof(connectionString));
        }

        return services;
    }
}