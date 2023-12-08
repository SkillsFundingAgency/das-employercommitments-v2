using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.CommitmentsV2.Api.Client.Configuration;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class ConfigurationServiceRegistrations
{
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        
        services.AddConfigurationFor<AccountIdHashingConfiguration>(configuration, ConfigurationKeys.AccountIdHashingConfiguration);
        services.AddConfigurationFor<AuthenticationConfiguration>(configuration, ConfigurationKeys.AuthenticationConfiguration);
        services.AddConfigurationFor<CommitmentsClientApiConfiguration>(configuration, ConfigurationKeys.CommitmentsApiClientConfiguration);
        
        services.AddConfigurationFor<CommitmentPermissionsApiClientConfiguration>(configuration, ConfigurationKeys.CommitmentsApiClientConfiguration);
        services.AddConfigurationFor<EmployerFeaturesConfiguration>(configuration, ConfigurationKeys.EmployerFeaturesConfiguration);
        
        services.AddConfigurationFor<EmployerCommitmentsV2Configuration>(configuration, ConfigurationKeys.EmployerCommitmentsV2);
        services.AddConfigurationFor<PublicAccountIdHashingConfiguration>(configuration, ConfigurationKeys.PublicAccountIdHashingConfiguration);
        services.AddConfigurationFor<PublicAccountLegalEntityIdHashingConfiguration>(configuration, ConfigurationKeys.PublicAccountLegalEntityIdHashingConfiguration);
        services.AddConfigurationFor<EncodingConfig>(configuration, ConfigurationKeys.Encoding);
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