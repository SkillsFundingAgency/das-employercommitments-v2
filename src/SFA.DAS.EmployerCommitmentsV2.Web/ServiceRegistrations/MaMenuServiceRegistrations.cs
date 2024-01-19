using Microsoft.Extensions.Configuration;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class MaMenuServiceRegistrations
{
    public static IServiceCollection AddDasMaMenuConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var commitmentsConfiguration = configuration
            .GetSection(ConfigurationKeys.EmployerCommitmentsV2)
            .Get<EmployerCommitmentsV2Configuration>();

        if (commitmentsConfiguration.UseGovSignIn)
        {
            services.AddMaMenuConfiguration(RouteNames.SignOut, configuration["ResourceEnvironmentName"]);
        }
        else
        {
            var authenticationConfiguration = configuration.GetSection(ConfigurationKeys.AuthenticationConfiguration).Get<AuthenticationConfiguration>();

            services.AddMaMenuConfiguration(RouteNames.SignOut, authenticationConfiguration.ClientId, configuration["ResourceEnvironmentName"]);    
        }
            
        return services;
    }
}