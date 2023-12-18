using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Client;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class CommitmentPermissionsApiClientServiceRegistrations
{
    public static IServiceCollection AddCommitmentPermissionsApiClient(this IServiceCollection services)
    {
        services.AddSingleton<ICommitmentPermissionsApiClientFactory, CommitmentPermissionsApiClientFactory>();
        services.AddSingleton<ICommitmentPermissionsApiClient>(provider =>
        {
            var factory = provider.GetService<ICommitmentPermissionsApiClientFactory>();
            return factory.CreateClient();
        });

        return services;
    }
}