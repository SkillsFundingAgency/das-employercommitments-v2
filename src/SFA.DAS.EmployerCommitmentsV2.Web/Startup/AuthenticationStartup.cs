using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Cookies;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.GovUK.Auth.Configuration;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class AuthenticationStartup
    {
        public static IServiceCollection AddDasEmployerAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var commitmentsConfiguration = configuration.GetSection(ConfigurationKeys.EmployerCommitmentsV2)
                .Get<EmployerCommitmentsV2Configuration>();
            if (commitmentsConfiguration.UseGovUkSignIn)
            {
                var govConfig = configuration.GetSection(ConfigurationKeys.GovUkSignInConfiguration);
                services.Configure<GovUkOidcConfiguration>(configuration.GetSection("GovUkOidcConfiguration"));
                services.AddTransient<ICustomClaims, EmployerUserAccountPostAuthenticationHandler>();
                services.AddAndConfigureGovUkAuthentication(govConfig,
                    $"{typeof(AuthenticationStartup).Assembly.GetName().Name}.Auth",
                    typeof(EmployerUserAccountPostAuthenticationHandler));
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
}