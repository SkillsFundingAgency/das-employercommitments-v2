using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Cookies;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.GovUK.Auth.Authentication;
using SFA.DAS.GovUK.Auth.Configuration;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class AuthenticationServiceRegistrations
{
    public static IServiceCollection AddDasEmployerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var commitmentsConfiguration = configuration.GetSection(ConfigurationKeys.EmployerCommitmentsV2)
            .Get<EmployerCommitmentsV2Configuration>();

        if (commitmentsConfiguration.UseGovSignIn)
        {
            ConfigureGovSignInAuth(services, configuration);
        }
        else
        {
            ConfigureEmployerAuth(services, configuration);
        }

        return services;
    }

    private static void ConfigureGovSignInAuth(IServiceCollection services, IConfiguration configuration)
    {
        var govConfig = configuration.GetSection(ConfigurationKeys.GovUkSignInConfiguration);
        services.Configure<GovUkOidcConfiguration>(configuration.GetSection("GovUkOidcConfiguration"));

        govConfig["ResourceEnvironmentName"] = configuration["ResourceEnvironmentName"];
        govConfig["StubAuth"] = configuration["StubAuth"];

        services.AddTransient<ICustomClaims, EmployerAccountPostAuthenticationClaimsHandler>();
        services.AddSingleton<IAuthorizationHandler, AccountActiveAuthorizationHandler>();
        services.AddSingleton<IStubAuthenticationService, StubAuthenticationService>();

        services.AddAndConfigureGovUkAuthentication(govConfig,
            typeof(EmployerAccountPostAuthenticationClaimsHandler),
            "",
            "/service/SignIn-Stub");
    }

    private static void ConfigureEmployerAuth(IServiceCollection services, IConfiguration configuration)
    {
        var authenticationConfiguration = configuration.GetSection(ConfigurationKeys.AuthenticationConfiguration).Get<AuthenticationConfiguration>();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.AccessDeniedPath = "/error?statuscode=403";
                options.Cookie.Name = CookieNames.Authentication;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            })
            .AddOpenIdConnect(connectOptions =>
            {
                connectOptions.Authority = authenticationConfiguration.Authority;
                connectOptions.ClientId = authenticationConfiguration.ClientId;
                connectOptions.ClientSecret = authenticationConfiguration.ClientSecret;
                connectOptions.MetadataAddress = authenticationConfiguration.MetadataAddress;
                connectOptions.ResponseType = "code";
                connectOptions.UsePkce = false;

                connectOptions.ClaimActions.MapUniqueJsonKey("sub", "id");

                connectOptions.Events.OnRemoteFailure = failureContext =>
                {
                    if (failureContext.Failure.Message.Contains("Correlation failed"))
                    {
                        failureContext.Response.Redirect("/");
                        failureContext.HandleResponse();
                    }

                    return Task.CompletedTask;
                };
            });
    }
}