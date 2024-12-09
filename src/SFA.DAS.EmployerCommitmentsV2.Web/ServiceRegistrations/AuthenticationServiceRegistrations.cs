using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.GovUK.Auth.Authentication;
using SFA.DAS.GovUK.Auth.Configuration;
using SFA.DAS.GovUK.Auth.Models;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class AuthenticationServiceRegistrations
{
    public static IServiceCollection AddDasEmployerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureGovSignInAuth(services, configuration);
        return services;
    }

    private static void ConfigureGovSignInAuth(IServiceCollection services, IConfiguration configuration)
    {
        var govConfig = configuration.GetSection(ConfigurationKeys.GovUkSignInConfiguration);
        services.Configure<GovUkOidcConfiguration>(configuration.GetSection("GovUkOidcConfiguration"));

        govConfig["ResourceEnvironmentName"] = configuration["ResourceEnvironmentName"];
        govConfig["StubAuth"] = configuration["StubAuth"];

        services.AddSingleton<IAuthorizationHandler, AccountActiveAuthorizationHandler>();
        services.AddSingleton<IStubAuthenticationService, StubAuthenticationService>();

        services.AddAndConfigureGovUkAuthentication(govConfig, new AuthRedirects
        {
            SignedOutRedirectUrl = "",
            LocalStubLoginPath = "/service/SignIn-Stub"
        }, null, typeof(UserAccountService));
    }
}