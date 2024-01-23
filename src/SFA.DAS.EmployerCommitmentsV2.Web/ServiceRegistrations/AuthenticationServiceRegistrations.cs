using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Client;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Infrastructure;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Commitments;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;
using SFA.DAS.EmployerCommitmentsV2.Web.Cookies;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.GovUK.Auth.Authentication;
using SFA.DAS.GovUK.Auth.Configuration;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class AuthenticationServiceRegistrations
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddSingleton<CommitmentsAuthorisationHandler>();
        services.AddSingleton<ICommitmentsAuthorisationHandler, CommitmentsAuthorisationHandler>();
        services.AddTransient<ICommitmentPermissionsApiClientFactory, CommitmentPermissionsApiClientFactory>();
        services.AddSingleton(serviceProvider => serviceProvider.GetService<ICommitmentPermissionsApiClientFactory>().CreateClient());

        services.AddTransient<ICustomClaims, EmployerAccountPostAuthenticationClaimsHandler>();

        services.AddSingleton<IEmployerAccountAuthorisationHandler, EmployerAccountAuthorisationHandler>();

        services.AddSingleton<IAuthorizationHandler, AccountActiveAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, EmployerAccountAllRolesAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, CommitmentAccessApprenticeshipAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, CommitmentAccessCohortAuthorizationHandler>();

        services.AddTransient<IAuthorizationContext, AuthorizationContext>();
        services.AddSingleton<IAuthorizationContextProvider, AuthorizationContextProvider>();

        services.AddSingleton<IStubAuthenticationService, StubAuthenticationService>();

        AddAuthorizationPolicies(services);

        return services;
    }

    public static IServiceCollection AddDasEmployerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var commitmentsConfiguration = configuration.GetSection(ConfigurationKeys.EmployerCommitmentsV2)
            .Get<EmployerCommitmentsV2Configuration>();
        
        if (commitmentsConfiguration.UseGovSignIn)
        {
            var govConfig = configuration.GetSection(ConfigurationKeys.GovUkSignInConfiguration);
            services.Configure<GovUkOidcConfiguration>(configuration.GetSection("GovUkOidcConfiguration"));
           
            govConfig["ResourceEnvironmentName"] = configuration["ResourceEnvironmentName"];
            govConfig["StubAuth"] = configuration["StubAuth"];
            
            services.AddTransient<ICustomClaims, EmployerAccountPostAuthenticationClaimsHandler>();
            services.AddAndConfigureGovUkAuthentication(govConfig,
                typeof(EmployerAccountPostAuthenticationClaimsHandler),
                "",
                "/service/SignIn-Stub");

            services.AddSingleton<IAuthorizationHandler, AccountActiveAuthorizationHandler>();
            services.AddSingleton<IStubAuthenticationService, StubAuthenticationService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "HasActiveAccount"
                    , policy =>
                    {
                        policy.Requirements.Add(new AccountActiveRequirement());
                        policy.Requirements.Add(new UserIsInAccountRequirement());
                        policy.RequireAuthenticatedUser();
                    });
            });
        }
        else
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

        return services;
    }

    private static void AddAuthorizationPolicies(IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.HasEmployerViewerTransactorOwnerAccount, policy =>
            {
                policy.RequireClaim(EmployerClaims.AccountsClaimsTypeIdentifier);
                policy.Requirements.Add(new AccountActiveRequirement());
                policy.Requirements.Add(new EmployerAccountAllRolesRequirement());
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy(PolicyNames.AccessApprenticeship, policy =>
            {
                policy.Requirements.Add(new AccountActiveRequirement());
                policy.Requirements.Add(new AccessApprenticeshipRequirement());
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy(PolicyNames.AccessCohort, policy =>
            {
                policy.Requirements.Add(new AccountActiveRequirement());
                policy.Requirements.Add(new AccessCohortRequirement());
                policy.RequireAuthenticatedUser();
            });
        });
    }
}