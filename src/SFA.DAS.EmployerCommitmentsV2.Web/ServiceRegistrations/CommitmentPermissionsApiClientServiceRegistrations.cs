using Microsoft.Extensions.Configuration;
using SFA.DAS.CommitmentsV2.Api.Client.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class CommitmentPermissionsApiClientServiceRegistrations
{
    public static IServiceCollection AddCommitmentPermissionsApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICommitmentPermissionsApiClientFactory, CommitmentPermissionsApiClientFactory>();
        services.AddSingleton<ICommitmentPermissionsApiClient>(provider =>
        {
            ICommitmentPermissionsApiClientFactory factory;

            if (configuration.UseLocalRegistry())
            {
                var config = provider.GetService<CommitmentsClientApiConfiguration>();
                var loggerFactory = provider.GetService<ILoggerFactory>();
                factory = new LocalDevApiClientFactory(config, loggerFactory);
            }
            else
            {
                factory = provider.GetService<ICommitmentPermissionsApiClientFactory>();
            }
            
            return factory.CreateClient();
        });

        return services;
    }
}