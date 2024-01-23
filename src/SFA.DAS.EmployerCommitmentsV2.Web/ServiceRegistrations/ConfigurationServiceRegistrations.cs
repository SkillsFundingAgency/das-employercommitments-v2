using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SFA.DAS.CommitmentsV2.Api.Client.Configuration;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerAccounts.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerUrlHelper.Configuration;
using SFA.DAS.Encoding;
using SFA.DAS.GovUK.Auth.Configuration;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class ConfigurationServiceRegistrations
{
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.AddConfigurationFor<CommitmentsClientApiConfiguration>(configuration, ConfigurationKeys.CommitmentsApiClientConfiguration);

        services.AddConfigurationFor<CommitmentPermissionsApiClientConfiguration>(configuration, ConfigurationKeys.CommitmentsApiClientConfiguration);
        services.AddConfigurationFor<EmployerUrlHelperConfiguration>(configuration, ConfigurationKeys.EmployerUrlConfiguration);

        services.AddConfigurationFor<EmployerCommitmentsV2Configuration>(configuration, ConfigurationKeys.EmployerCommitmentsV2);
        services.AddConfigurationFor<EmployerAccountsApiClientConfiguration>(configuration, ConfigurationKeys.AccountApiConfiguration);
        services.AddConfigurationFor<GovUkOidcConfiguration>(configuration, ConfigurationKeys.AccountApiConfiguration);

        var encodingConfigJson = configuration.GetSection(ConfigurationKeys.Encoding).Value;
        var encodingConfig = JsonConvert.DeserializeObject<EncodingConfig>(encodingConfigJson);
        services.AddSingleton(encodingConfig);

        services.AddConfigurationFor<AccountApiConfiguration>(configuration, ConfigurationKeys.AccountApiConfiguration);
        services.AddConfigurationFor<ZenDeskConfiguration>(configuration, ConfigurationKeys.ZenDeskConfiguration);
        services.AddConfigurationFor<ApprovalsApiClientConfiguration>(configuration, ConfigurationKeys.ApprovalsApiClientConfiguration);

        return services;
    }

    private static void AddConfigurationFor<T>(this IServiceCollection services, IConfiguration configuration,
        string key) where T : class => services.AddSingleton(GetConfigurationFor<T>(configuration, key));

    private static T GetConfigurationFor<T>(IConfiguration configuration, string name)
    {
        var section = configuration.GetSection(name);
        return section.Get<T>();
    }
}