using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.Authorization;
using SFA.DAS.Authorization.CommitmentPermissions;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization
{
    public class AuthorizationContextProvider : IAuthorizationContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEncodingService _encodingService;
        private readonly IAuthenticationService _authenticationService;

        public AuthorizationContextProvider(IHttpContextAccessor httpContextAccessor, IEncodingService encodingService, IAuthenticationService authenticationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _encodingService = encodingService;
            _authenticationService = authenticationService;
        }

        public IAuthorizationContext GetAuthorizationContext()
        {
            var authorizationContext = new AuthorizationContext();
            var accountId = GetAccountId();
            var cohortId = GetCohortId();
            var userRef = GetUserRef();

            if (cohortId.HasValue)
            {
                authorizationContext.Set("CohortId", cohortId.Value);
            }

            if (accountId.HasValue)
            {
                authorizationContext.Set("AccountId", accountId.Value);
            }

            if (accountId.HasValue && userRef.HasValue)
            {
                authorizationContext.AddEmployerUserRoleValues(accountId.Value, userRef.Value);
            }

            if (accountId.HasValue && cohortId.HasValue)
            {
                authorizationContext.AddCommitmentPermissionValues(cohortId.Value, Party.Employer, accountId.Value);
            }

            return authorizationContext;
        }

        private long? GetAccountId()
        {
            return GetAndDecodeValueIfExists(RouteValueKeys.AccountHashedId, EncodingType.AccountId);
        }

        private long? GetCohortId()
        {
            return GetAndDecodeValueIfExists(RouteValueKeys.CohortReference, EncodingType.CohortReference);
        }

        private Guid? GetUserRef()
        {
            if (!_authenticationService.IsUserAuthenticated())
            {
                return null;
            }

            if (!_authenticationService.TryGetUserClaimValue(EmployeeClaims.Id, out var idClaimValue))
            {
                throw new UnauthorizedAccessException();
            }

            if (!Guid.TryParse(idClaimValue, out var id))
            {
                throw new UnauthorizedAccessException();
            }

            return id;
        }

        private long? GetAndDecodeValueIfExists(string keyName, EncodingType encodedType)
        {
            if (!TryGetValueFromHttpContext(keyName, out var encodedValue))
            {
                return null;
            }

            if (!_encodingService.TryDecode(encodedValue, encodedType, out var id))
            {
                throw new UnauthorizedAccessException();
            }

            return id;
        }

        private bool TryGetValueFromHttpContext(string key, out string value)
        {
            value = null;

            if (_httpContextAccessor.HttpContext.GetRouteData().Values.TryGetValue(key, out var routeValue))
            {
                value = (string)routeValue;
            }
            else if (_httpContextAccessor.HttpContext.Request.Query.TryGetValue(key, out var queryStringValue))
            {
                value = queryStringValue;
            }
            else if (_httpContextAccessor.HttpContext.Request.HasFormContentType && _httpContextAccessor.HttpContext.Request.Form.TryGetValue(key, out var formValue))
            {
                value = formValue;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return true;
        }
    }
}