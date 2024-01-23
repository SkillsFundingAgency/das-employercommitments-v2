using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution.Microsoft;
using SFA.DAS.Authorization.DependencyResolution.Microsoft;
using SFA.DAS.Authorization.EmployerFeatures.DependencyResolution.Microsoft;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class AuthorizationStartup
    {
        public static IServiceCollection AddDasAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
			BearerTokenProvider.SetSigningKey(configuration["SFA.DAS.EmployerCommitmentsV2:UserBearerTokenSigningKey"]);

			return services.AddAuthorization<AuthorizationContextProvider>()
                .AddCommitmentPermissionsAuthorization()
                .AddEmployerFeaturesAuthorization();
        }
    }
}