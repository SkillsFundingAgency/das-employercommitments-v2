using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Newtonsoft.Json;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authentication;

public class EmployerAccountPostAuthenticationClaimsHandler(
    IUserAccountService userAccountService, 
    EmployerCommitmentsV2Configuration employerAccountsConfiguration) : ICustomClaims
{
    // To allow unit testing
    public int MaxPermittedNumberOfAccountsOnClaim { get; set; } = WebConstants.MaxNumberOfEmployerAccountsAllowedOnClaim;

    public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
    {
        var claims = new List<Claim>();

        string userId;
        string email;

        if (employerAccountsConfiguration.UseGovSignIn)
        {
            userId = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                .Value;
            email = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(ClaimTypes.Email))
                .Value;
            claims.Add(new Claim(EmployeeClaims.IdamsUserEmailClaimTypeIdentifier, email));
        }
        else
        {
            userId = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(EmployeeClaims.IdamsUserIdClaimTypeIdentifier))
                .Value;

            email = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(EmployeeClaims.IdamsUserEmailClaimTypeIdentifier)).Value;

            claims.AddRange(tokenValidatedContext.Principal.Claims);
            claims.Add(new Claim(EmployeeClaims.IdamsUserIdClaimTypeIdentifier, userId));
        }

        var result = await userAccountService.GetUserAccounts(userId, email);

        // Some users have 100's of employer accounts. The claims cannot handle that volume of data.
        if (result.EmployerAccounts.Count() <= MaxPermittedNumberOfAccountsOnClaim)
        {
            var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
            var associatedAccountsClaim = new Claim(EmployeeClaims.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json);
            claims.Add(associatedAccountsClaim);    
        }
        
        if (!employerAccountsConfiguration.UseGovSignIn)
        {
            return claims;
        }

        if (result.IsSuspended)
        {
            claims.Add(new Claim(ClaimTypes.AuthorizationDecision, "Suspended"));
        }

        claims.Add(new Claim(EmployeeClaims.IdamsUserIdClaimTypeIdentifier, result.EmployerUserId));

        if (!string.IsNullOrEmpty(result.FirstName) && !string.IsNullOrEmpty(result.LastName))
        {
            claims.Add(new Claim(EmployeeClaims.IdamsUserDisplayNameClaimTypeIdentifier, result.FirstName + " " + result.LastName));
        }

        result.EmployerAccounts
            .Where(c => c.Role.Equals("owner", StringComparison.CurrentCultureIgnoreCase) || c.Role.Equals("transactor", StringComparison.CurrentCultureIgnoreCase))
            .ToList().ForEach(u => claims.Add(new Claim(EmployeeClaims.AccountIdClaimTypeIdentifier, u.AccountId)));

        return claims;
    }
}