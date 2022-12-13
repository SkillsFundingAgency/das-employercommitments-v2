using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class MaMenuStartup
    {
        public static IServiceCollection AddDasMaMenuConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationConfiguration = configuration.GetSection(ConfigurationKeys.AuthenticationConfiguration).Get<AuthenticationConfiguration>();

            services.AddMaMenuConfiguration(RouteNames.SignOut, authenticationConfiguration.ClientId, configuration["ResourceEnvironmentName"]);

            return services;
        }
    }
}