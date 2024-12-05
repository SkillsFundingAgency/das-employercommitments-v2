using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.GovUK.Auth.Employer;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Middleware;

/// <summary>
/// Middleware that ensures a user's identity contains all necessary claims, even if the user logged in on another subdomain.
/// 
/// <para>
/// This middleware checks for missing claims in the user's identity on each request, retrieves them if necessary, 
/// and updates the identity to ensure consistent access to required resources.
/// </para>
/// <para>
/// Primarily useful for scenarios where claims may not propagate across subdomains, 
/// this class supplements the identity with any additional claims needed post-login.
/// </para>
/// </summary>
public class MissingClaimsMiddleware(RequestDelegate next, IGovAuthEmployerAccountService userAccountService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if(context.User?.Identity?.IsAuthenticated == true && HasMissingAccountClaims(context))
        {
            await EnrichWithAccountClaims(context);
        }

        await next(context);
    }

    private async Task EnrichWithAccountClaims(HttpContext context)
    {
        var user = context.User;

        var userId = user.Claims.First(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        var email = user.Claims.First(c => c.Type.Equals(ClaimTypes.Email)).Value;

        var result = await userAccountService.GetUserAccounts(userId, email);

        if (user.Identity is ClaimsIdentity identity)
        {
            result.EmployerAccounts
                .Where(c => c.Role.Equals("owner", StringComparison.CurrentCultureIgnoreCase) || c.Role.Equals("transactor", StringComparison.CurrentCultureIgnoreCase))
                .ToList().ForEach(u => identity.AddClaim(new Claim(EmployeeClaims.AccountIdClaimTypeIdentifier, u.AccountId)));
     
            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }
    }

    private static bool HasMissingAccountClaims(HttpContext context)
    {
        if (context.User.Claims.Any(c => c.Type == EmployeeClaims.AccountIdClaimTypeIdentifier))
        {
            return false;
        }
        return true;
    }
}