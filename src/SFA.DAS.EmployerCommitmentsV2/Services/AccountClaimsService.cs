using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerCommitmentsV2.Extensions;
using SFA.DAS.GovUK.Auth.Employer;
using EmployerClaims = SFA.DAS.EmployerCommitmentsV2.Infrastructure.EmployerClaims;

namespace SFA.DAS.EmployerCommitmentsV2.Services;

public interface IAccountClaimsService
{
    Task<Dictionary<string, EmployerUserAccountItem>> GetAssociatedAccounts(bool forceRefresh);
}

public class AccountClaimsService(IGovAuthEmployerAccountService accountsService, IHttpContextAccessor httpContextAccessor, ILogger<AccountClaimsService> logger) : IAccountClaimsService
{
    // To allow unit testing
    public int MaxPermittedNumberOfAccountsOnClaim { get; set; } = Constants.WebConstants.MaxNumberOfEmployerAccountsAllowedOnClaim;

    /// <summary>
    /// Retrieves a users associated employer accounts from claims.
    /// If the claim is null, the data will be pulled from UserAccountService and persisted to the claims for caching purposes.
    /// </summary>
    /// <param name="forceRefresh">Forces data to be refreshed from UserAccountsService and persisted to user claims regardless of claims state.</param>
    /// <returns>Dictionary of string, EmployerUserAccountItem</returns>
    public async Task<Dictionary<string, EmployerUserAccountItem>> GetAssociatedAccounts(bool forceRefresh)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.ClaimsAreEmpty())
        {
            return null;
        }
        
        var employerAccountsClaim = user.FindFirst(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));

        if (!forceRefresh && employerAccountsClaim != null)
        {
            try
            {
                var accountsFromClaim = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(employerAccountsClaim.Value);

                // Some users have 100's of employer accounts. The claims cannot handle that volume of data,
                // so the claim may have been added for authorization purposes, but the claim itself is empty.
                if (accountsFromClaim != null && accountsFromClaim.Count > 0)
                {
                    return accountsFromClaim;
                }
            }
            catch (JsonSerializationException e)
            {
                logger.LogError(e, "Could not retrieve employer accounts for user");
                throw;
            }
        }

        var userClaim = user.Claims.First(c => c.Type.Equals(ClaimTypes.NameIdentifier));
        var email = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;
        var userId = userClaim.Value;

        var result = await accountsService.GetUserAccounts(userId, email);
        var associatedAccounts = result.EmployerAccounts.ToDictionary(k => k.AccountId);

        if (forceRefresh)
        {
            PersistToClaims(associatedAccounts, employerAccountsClaim, userClaim);
        }

        return associatedAccounts;
    }
    
    private void PersistToClaims(Dictionary<string, EmployerUserAccountItem> associatedAccounts, Claim employerAccountsClaim, Claim userClaim)
    {
        // Some users have 100's of employer accounts. The claims cannot handle that volume of data.
        var accountsAsJson = JsonConvert.SerializeObject(associatedAccounts.Count <= MaxPermittedNumberOfAccountsOnClaim
            ? associatedAccounts
            : new Dictionary<string, EmployerUserAccountItem>());

        var associatedAccountsClaim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json);

        if (employerAccountsClaim != null)
        {
            userClaim.Subject!.RemoveClaim(employerAccountsClaim);
        }

        userClaim.Subject!.AddClaim(associatedAccountsClaim);
    }
}