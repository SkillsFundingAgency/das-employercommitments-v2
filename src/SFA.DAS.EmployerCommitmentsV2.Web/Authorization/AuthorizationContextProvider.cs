using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.Authorization.CommitmentPermissions.Context;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.EmployerUserRoles.Context;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization;

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
        var apprenticeshipId = GetApprenticeshipId();

        CopyRouteValueToAuthorizationContextIfAvailable(authorizationContext, accountId, AuthorizationContextKeys.AccountId);
        CopyRouteValueToAuthorizationContextIfAvailable(authorizationContext, cohortId, AuthorizationContextKeys.CohortId);
        CopyRouteValueToAuthorizationContextIfAvailable(authorizationContext, GetAccountLegalEntityHashedId(), AuthorizationContextKeys.AccountLegalEntityId);
        CopyRouteValueToAuthorizationContextIfAvailable(authorizationContext, GetDraftApprenticeshipId(), AuthorizationContextKeys.DraftApprenticeshipId);
        CopyRouteValueToAuthorizationContextIfAvailable(authorizationContext, GetTransferSenderId(), AuthorizationContextKeys.DecodedTransferSenderId);
        CopyRouteValueToAuthorizationContextIfAvailable(authorizationContext, GetTransferRequestId(), AuthorizationContextKeys.TransferRequestId);
        CopyRouteValueToAuthorizationContextIfAvailable(authorizationContext, apprenticeshipId, AuthorizationContextKeys.ApprenticeshipId);
        CopyRouteValueToAuthorizationContextIfAvailable(authorizationContext, GetPledgeApplicationId(), AuthorizationContextKeys.PledgeApplicationId);

        if (accountId.HasValue)
        { 
            if(userRef.HasValue)
            {
                authorizationContext.AddEmployerUserRoleValues(accountId.Value, userRef.Value);
            } 
                
            if (cohortId.HasValue)
            {
                authorizationContext.AddCommitmentPermissionValues(cohortId.Value, Party.Employer, accountId.Value);
            }

            if (apprenticeshipId.HasValue)
            {
                authorizationContext.AddApprenticeshipPermissionValues(apprenticeshipId.Value, Party.Employer, accountId.Value);
            }
        }

        return authorizationContext;
    }

    private void CopyRouteValueToAuthorizationContextIfAvailable<T>(IAuthorizationContext ctx, T? value, string name) where T : struct
    {
        if (value.HasValue)
        {
            ctx.Set(name, value.Value);
        }
    }

    private long? GetAccountId()
    {
        return GetAndDecodeValueIfExists(RouteValueKeys.AccountHashedId, EncodingType.AccountId);
    }

    private long? GetCohortId()
    {
        return GetAndDecodeValueIfExists(RouteValueKeys.CohortReference, EncodingType.CohortReference);
    }

    private long? GetAccountLegalEntityHashedId()
    {
        return GetAndDecodeValueIfExists(RouteValueKeys.AccountLegalEntityHashedId, EncodingType.PublicAccountLegalEntityId);
    }

    private long? GetDraftApprenticeshipId()
    {
        return GetAndDecodeValueIfExists(RouteValueKeys.DraftApprenticeshipHashedId, EncodingType.ApprenticeshipId);
    }

    private long? GetApprenticeshipId()
    {
        return GetAndDecodeValueIfExists(RouteValueKeys.ApprenticeshipHashedId, EncodingType.ApprenticeshipId);
    }

    private long? GetTransferSenderId()
    {
        return GetAndDecodeValueIfExists(RouteValueKeys.TransferSenderId, EncodingType.PublicAccountId);
    }

    private long? GetPledgeApplicationId()
    {
        return GetAndDecodeValueIfExists(RouteValueKeys.PledgeApplicationId, EncodingType.PledgeApplicationId);
    }

    private long? GetTransferRequestId()
    {
        return GetAndDecodeValueIfExists(RouteValueKeys.TransferRequestHashedId, EncodingType.TransferRequestId);
    }

    private Guid? GetUserRef()
    {
        if (!_authenticationService.IsUserAuthenticated())
        {
            return null;
        }

        if (!_authenticationService.TryGetUserClaimValue(EmployeeClaims.IdamsUserIdClaimTypeIdentifier, out var idClaimValue))
        {
            throw new UnauthorizedAccessException($"Failed to get value for claim '{EmployeeClaims.IdamsUserIdClaimTypeIdentifier}'");
        }

        if (!Guid.TryParse(idClaimValue, out var id))
        {
            throw new UnauthorizedAccessException($"Failed to parse value '{idClaimValue}' for claim '{EmployeeClaims.IdamsUserIdClaimTypeIdentifier}'");
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
            throw new UnauthorizedAccessException($"Failed to decode '{keyName}' value '{encodedValue}' using encoding type '{encodedType}'");
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