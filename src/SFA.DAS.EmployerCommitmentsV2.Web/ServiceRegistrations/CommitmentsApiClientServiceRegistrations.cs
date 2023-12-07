using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Client.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class CommitmentsApiClientServiceRegistrations
{
    public static IServiceCollection AddCommitmentsApiClient(this IServiceCollection services, IConfiguration configuration)
    {
       services.AddSingleton<ICommitmentsApiClientFactory>(x =>
        {
            var config = x.GetService<CommitmentsClientApiConfiguration>();
            var loggerFactory = x.GetService<ILoggerFactory>();
            
            if (configuration.UseLocalRegistry())
            {
                return new LocalDevApiClientFactory(config, loggerFactory);
            }
            
            return new CommitmentsApiClientFactory(config, loggerFactory);
        });
        
        services.AddTransient(provider => provider.GetRequiredService<ICommitmentsApiClientFactory>().CreateClient());

        return services;
    }
}