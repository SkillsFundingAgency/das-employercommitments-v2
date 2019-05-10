using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.Cookies;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class AuthenticationStartup
    {
        public static IServiceCollection AddDasEmployerAuthentication(this IServiceCollection services, IConfiguration configuration)
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
            
            return services;
        }
        
        public static void RequireAuthenticatedUser(this MvcOptions mvcOptions)
        {
            var authorizationPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            var authorizeFilter = new AuthorizeFilter(authorizationPolicy);
            
            mvcOptions.Filters.Add(authorizeFilter);
        }
    }
}