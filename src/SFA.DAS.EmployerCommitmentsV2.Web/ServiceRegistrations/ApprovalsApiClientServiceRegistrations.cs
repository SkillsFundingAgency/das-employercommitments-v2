using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Factories;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class ApprovalsApiClientServiceRegistrations
{
    public static IServiceCollection AddApprovalsApiClient(this IServiceCollection services)
    {
        services.AddTransient<IApprovalsApiClientFactory, ApprovalsApiClientFactory>();
        services.AddSingleton<IApprovalsApiClient>(provider =>
        {
            var factory = provider.GetService<IApprovalsApiClientFactory>();
            return factory.CreateClient();
        });

        return services;
    }
}