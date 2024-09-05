using Microsoft.Extensions.Configuration;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class MaMenuServiceRegistrations
{
    public static IServiceCollection AddDasMaMenuConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMaMenuConfiguration(RouteNames.SignOut, configuration["ResourceEnvironmentName"]);
        return services;
    }
}