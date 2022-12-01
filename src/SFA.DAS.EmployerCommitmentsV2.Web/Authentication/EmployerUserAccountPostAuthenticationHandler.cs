using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authentication
{
    public class EmployerUserAccountPostAuthenticationHandler : ICustomClaims
    {
        private readonly IApprovalsApiClient _approvalsApiClient;

        public EmployerUserAccountPostAuthenticationHandler(IApprovalsApiClient approvalsApiClient)
        {
            _approvalsApiClient = approvalsApiClient;
        }
        public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
        {
            var userId = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                .Value;
            var email = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(ClaimTypes.Email))
                .Value;
            
            var result = await _approvalsApiClient.GetEmployerUserAccounts(email, userId);
            
            var claims = new List<Claim>
            {
                new Claim(EmployeeClaims.Id, result.EmployerUserId),
                new Claim(EmployeeClaims.Email, email),
                new Claim(EmployeeClaims.Name, $"{result.FirstName} {result.LastName}")
            };
            return claims;
        }
    }
}

