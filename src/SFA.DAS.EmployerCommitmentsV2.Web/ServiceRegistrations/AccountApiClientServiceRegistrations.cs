using SFA.DAS.CommitmentsV2.Shared.Services;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Configuration;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class AccountApiClientServiceRegistrations
{
    public static IServiceCollection AddAccountsApiClient(this IServiceCollection services, EmployerCommitmentsV2Configuration configuration)
    {
        if (configuration.UseStubEmployerAccountsApiClient)
        {
            services.AddSingleton<IAccountApiClient, StubAccountApiClient>();
        }
        else
        {
            services.AddTransient<IAccountApiClient>(provider =>
            {
                var config = provider.GetService<AccountApiConfiguration>();
                return new AccountApiClient(config);
            });     
        }

        return services;
    }
}