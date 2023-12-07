using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution.Microsoft;
using SFA.DAS.Authorization.DependencyResolution.Microsoft;
using SFA.DAS.Authorization.EmployerFeatures.DependencyResolution.Microsoft;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.GovUK.Auth.Authentication;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class AuthorizationServiceRegistrations
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorization<AuthorizationContextProvider>()
            .AddCommitmentPermissionsAuthorization()
            .AddEmployerFeaturesAuthorization();
        
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IEmployerAccountAuthorisationHandler, EmployerAccountAuthorisationHandler>();
        services.AddTransient<IAuthorizationHandler, UserIsInAccountAuthorizationHandler>();

        return services;
    }
}