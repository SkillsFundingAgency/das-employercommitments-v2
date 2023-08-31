using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Newtonsoft.Json;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authentication
{
    public class EmployerAccountPostAuthenticationClaimsHandler : ICustomClaims
    {
        private readonly IUserAccountService _userAccountService;
        private readonly EmployerCommitmentsV2Configuration _employerCommitmentsConfiguration;

        public EmployerAccountPostAuthenticationClaimsHandler(IUserAccountService userAccountService, EmployerCommitmentsV2Configuration employerAccountsConfiguration)
        {
            _userAccountService = userAccountService;
            _employerCommitmentsConfiguration = employerAccountsConfiguration;
        }
        public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
        {
            var claims = new List<Claim>();

            string userId;
            var email = string.Empty;

            if (_employerCommitmentsConfiguration.UseGovSignIn)
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

            var result = await _userAccountService.GetUserAccounts(userId, email);

            var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
            var associatedAccountsClaim = new Claim(EmployeeClaims.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json);
            claims.Add(associatedAccountsClaim);

            if (!_employerCommitmentsConfiguration.UseGovSignIn)
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


            return claims;
        }
    }
}

