using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Infrastructure;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Commitments;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;
using SFA.DAS.EmployerCommitmentsV2.Web.Cookies;
using SFA.DAS.EmployerCommitmentsV2.Web.ModelBinding;
using SFA.DAS.GovUK.Auth.Authentication;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class AuthenticationServiceRegistrations
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddTransient<ICustomClaims, EmployerAccountPostAuthenticationClaimsHandler>();
        services.AddSingleton<ICommitmentsAuthorisationHandler, CommitmentsAuthorisationHandler>();
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

    public static void AddAndConfigureEmployerAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var authenticationConfiguration = configuration.GetSection(ConfigurationKeys.AuthenticationConfiguration)
            .Get<AuthenticationConfiguration>();

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
            .AddOpenIdConnect(options =>
            {
                options.Authority = authenticationConfiguration.Authority;
                options.ClientId = authenticationConfiguration.ClientId;
                options.ClientSecret = authenticationConfiguration.ClientSecret;
                options.MetadataAddress = authenticationConfiguration.MetadataAddress;
                options.ResponseType = "code";
                options.UsePkce = false;

                options.ClaimActions.MapUniqueJsonKey("sub", "id");

                options.Events.OnRemoteFailure = remoteFailureContext =>
                {
                    if (!remoteFailureContext.Failure.Message.Contains("Correlation failed"))
                    {
                        return Task.CompletedTask;
                    }
                    
                    remoteFailureContext.Response.Redirect("/");
                    remoteFailureContext.HandleResponse();

                    return Task.CompletedTask;
                };
            });
    }
}