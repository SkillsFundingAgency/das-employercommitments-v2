using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Client;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Commitments;

public interface ICommitmentsAuthorisationHandler
{
    Task<bool> CanAccessCohort();
    Task<bool> CanAccessApprenticeship();
}

public class CommitmentsAuthorisationHandler : ICommitmentsAuthorisationHandler
{
    private readonly ICommitmentPermissionsApiClient _commitmentsApiClient;
    private readonly IAuthorizationContext _authorizationContext;

    public CommitmentsAuthorisationHandler(
        ICommitmentPermissionsApiClient commitmentsApiClient,
        IAuthorizationContext authorizationContext
    )
    {
        _commitmentsApiClient = commitmentsApiClient;
        _authorizationContext = authorizationContext;
    }

    public Task<bool> CanAccessCohort()
    {
        var permissionValues = GetPermissionValues(_authorizationContext);

        var request = new CohortAccessRequest
        {
            CohortId = permissionValues.CohortId,
            Party = permissionValues.Party,
            PartyId = permissionValues.PartyId
        };

        return _commitmentsApiClient.CanAccessCohort(request);
    }

    public Task<bool> CanAccessApprenticeship()
    {
        var permissionValues = GetPermissionValues(_authorizationContext);

        var request = new ApprenticeshipAccessRequest
        {
            ApprenticeshipId = permissionValues.ApprenticeshipId,
            Party = permissionValues.Party,
            PartyId = permissionValues.PartyId
        };

        return _commitmentsApiClient.CanAccessApprenticeship(request);
    }

    private static (long CohortId, long ApprenticeshipId, Party Party, long PartyId) GetPermissionValues(
        IAuthorizationContext authorizationContext)
    {
        authorizationContext.TryGet<long>(AuthorizationContextKey.CohortId, out var cohortId);
        authorizationContext.TryGet<long>(AuthorizationContextKey.ApprenticeshipId, out var apprenticeshipId);

        if (cohortId == 0 && apprenticeshipId == 0)
        {
            throw new KeyNotFoundException(
                $"At least one key of '{AuthorizationContextKey.CohortId}' or '{AuthorizationContextKey.ApprenticeshipId}' should be present in the authorization context");
        }

        return (cohortId,
                apprenticeshipId,
                authorizationContext.Get<Party>(AuthorizationContextKey.Party),
                authorizationContext.Get<long>(AuthorizationContextKey.PartyId)
            );
    }
}