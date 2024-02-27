using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Client;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Commitments;

public class CommitmentsAuthorisationHandler : ICommitmentsAuthorisationHandler
{
    private readonly ICommitmentPermissionsApiClient _commitmentsApiClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEncodingService _encodingService;

    public CommitmentsAuthorisationHandler(
        ICommitmentPermissionsApiClient commitmentsApiClient,
        IHttpContextAccessor httpContextAccessor,
        IEncodingService encodingService
    )
    {
        _commitmentsApiClient = commitmentsApiClient;
        _httpContextAccessor = httpContextAccessor;
        _encodingService = encodingService;
    }

    public Task<bool> CanAccessCohort()
    {
        var permissionValues = GetPermissionValues();

        var request = new CohortAccessRequest
        {
            CohortId = permissionValues.CohortId,
            Party = Party.Employer,
            PartyId = permissionValues.PartyId
        };

        return _commitmentsApiClient.CanAccessCohort(request);
    }

    public Task<bool> CanAccessApprenticeship()
    {
        var permissionValues = GetPermissionValues();

        var request = new ApprenticeshipAccessRequest
        {
            ApprenticeshipId = permissionValues.ApprenticeshipId,
            Party = Party.Employer,
            PartyId = permissionValues.PartyId
        };

        return _commitmentsApiClient.CanAccessApprenticeship(request);
    }

    private (long CohortId, long ApprenticeshipId, long PartyId) GetPermissionValues()
    {
        var cohortId = GetCohortId();
        var apprenticeshipId = GetApprenticeshipId();
        var accountId = GetAccountId();

        if (cohortId == 0 && apprenticeshipId == 0 && accountId == 0)
        {
            throw new KeyNotFoundException("At least one key of 'AccountId', 'CohortId' or 'ApprenticeshipId' should be present in the authorization context");
        }

        return (cohortId, apprenticeshipId, accountId);
    }

    private long GetAccountId()
    {
        return GetAndDecodeValueIfExists(RouteValueKeys.AccountHashedId, EncodingType.AccountId);
    }

    private long GetApprenticeshipId()
    {
        return GetAndDecodeValueIfExists(RouteValueKeys.ApprenticeshipHashedId, EncodingType.ApprenticeshipId);
    }

    private long GetCohortId()
    {
        return GetAndDecodeValueIfExists(RouteValueKeys.CohortReference, EncodingType.CohortReference);
    }

    private long GetAndDecodeValueIfExists(string keyName, EncodingType encodedType)
    {
        if (!TryGetValueFromHttpContext(keyName, out var encodedValue))
        {
            return 0;
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

        return !string.IsNullOrWhiteSpace(value);
    }
}