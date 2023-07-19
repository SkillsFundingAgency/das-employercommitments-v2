using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Cookies;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.GovUK.Auth.Authentication;
using SFA.DAS.GovUK.Auth.Configuration;
using SFA.DAS.GovUK.Auth.Extensions;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class AuthenticationStartup
    {
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
                services.AddTransient<ICustomClaims, EmployerUserAccountPostAuthenticationHandler>();
                services.AddAndConfigureGovUkAuthentication(govConfig,
                    typeof(EmployerUserAccountPostAuthenticationHandler),
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
                            policy.RequireAuthenticatedUser();
                        });
                });
            }
            else
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
            
            
            
            return services;
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
}