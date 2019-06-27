using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution;
using SFA.DAS.Authorization.DependencyResolution;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class AuthorizationStartup
    {
        public static IServiceCollection AddDasAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization<AuthorizationContextProvider>()
                .AddCommitmentPermissionsAuthorization();
            return services;
        }
    }
}