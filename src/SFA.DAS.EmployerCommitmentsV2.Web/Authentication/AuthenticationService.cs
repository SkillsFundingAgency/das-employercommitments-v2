using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => GetUserClaimAsString(EmployeeClaims.Id);
        public string UserName => GetUserClaimAsString(EmployeeClaims.Name);
        public string UserEmail => GetUserClaimAsString(EmployeeClaims.Email);

        public bool IsUserAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool TryGetUserClaimValue(string key, out string value)
        {
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(key);
            var exists = claim != null;

            value = exists ? claim.Value : null;

            return exists;
        }

        private string GetUserClaimAsString(string claim)
        {
            if (IsUserAuthenticated() && TryGetUserClaimValue(claim, out var value))
            {
                return value;
            }
            return null;
        }
    }
}