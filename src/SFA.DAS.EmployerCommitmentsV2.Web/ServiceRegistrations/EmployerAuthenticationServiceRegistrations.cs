using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Infrastructure;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;
using SFA.DAS.EmployerCommitmentsV2.Web.Cookies;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.GovUK.Auth.Authentication;
using SFA.DAS.GovUK.Auth.Configuration;
using SFA.DAS.GovUK.Auth.Extensions;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class EmployerAuthenticationServiceRegistrations
{
    public static IServiceCollection AddDasEmployerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var commitmentsConfiguration = configuration.GetSection(ConfigurationKeys.EmployerCommitmentsV2)
            .Get<EmployerCommitmentsV2Configuration>();
        
        if (commitmentsConfiguration.UseGovSignIn)
        {
            ConfigureGovSignIn(services, configuration);
        }
        else
        {
            ConfigureAuthorization(services, configuration);
        }
            
        return services;
    }

    private static void ConfigureGovSignIn(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ICustomClaims, EmployerAccountPostAuthenticationClaimsHandler>();
        services.AddAndConfigureGovUkAuthentication(configuration,
            typeof(EmployerAccountPostAuthenticationClaimsHandler),
            "",
            "/service/SignIn-Stub");

        services.AddSingleton<IAuthorizationHandler, AccountActiveAuthorizationHandler>();
        services.AddSingleton<IStubAuthenticationService, StubAuthenticationService>();
                
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.HasActiveAccount, policy =>
                {
                    policy.Requirements.Add(new AccountActiveRequirement());
                    policy.Requirements.Add(new EmployerAccountAllRolesRequirement());
                    policy.RequireAuthenticatedUser();
                });
            
            options.AddPolicy(
                PolicyNames.HasEmployerViewerTransactorOwnerAccount, policy =>
                {
                    policy.RequireClaim(EmployerClaims.AccountsClaimsTypeIdentifier);
                    policy.Requirements.Add(new AccountActiveRequirement());
                    policy.Requirements.Add(new EmployerAccountAllRolesRequirement());
                    policy.RequireAuthenticatedUser();
                });
        });
    }

    private static void ConfigureAuthorization(IServiceCollection services, IConfiguration configuration)
    {
        var authenticationConfiguration = configuration.GetSection(ConfigurationKeys.AuthenticationConfiguration).Get<AuthenticationConfiguration>();
            
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services
            .AddAuthentication(o =>
            {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(o =>
            {
                o.AccessDeniedPath = "/error?statuscode=403";
                o.Cookie.Name = CookieNames.Authentication;
                o.Cookie.SameSite = SameSiteMode.None;
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                o.SlidingExpiration = true;
            })
            .AddOpenIdConnect(o =>
            {
                o.Authority = authenticationConfiguration.Authority;
                o.ClientId = authenticationConfiguration.ClientId;
                o.ClientSecret = authenticationConfiguration.ClientSecret;
                o.MetadataAddress = authenticationConfiguration.MetadataAddress;
                o.ResponseType = "code";
                o.UsePkce = false;

                o.ClaimActions.MapUniqueJsonKey("sub", "id");
                        
                o.Events.OnRemoteFailure = c =>
                {
                    if (c.Failure.Message.Contains("Correlation failed"))
                    {
                        c.Response.Redirect("/");
                        c.HandleResponse();
                    }
        
                    return Task.CompletedTask;
                };
            });
    }
}
//TODO once upgraded to .net6 - this filter can be deleted
public class AccountActiveFilter : IActionFilter
{
    private readonly IConfiguration _configuration;

    public AccountActiveFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context != null)
        {
            var isAccountSuspended = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.AuthorizationDecision))?.Value;
            if (isAccountSuspended != null && isAccountSuspended.Equals("Suspended", StringComparison.CurrentCultureIgnoreCase))
            {
                context.HttpContext.Response.Redirect(RedirectExtension.GetAccountSuspendedRedirectUrl(_configuration["ResourceEnvironmentName"]));
            }
        }
    }
}