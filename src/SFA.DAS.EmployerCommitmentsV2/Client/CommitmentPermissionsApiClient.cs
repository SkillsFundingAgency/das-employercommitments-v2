using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Client;

public interface ICommitmentPermissionsApiClient
{
    Task<bool> CanAccessCohort(CohortAccessRequest request, CancellationToken cancellationToken = default);
    Task<bool> CanAccessApprenticeship(ApprenticeshipAccessRequest request, CancellationToken cancellationToken = default);
}

public class CommitmentPermissionsApiClient : ICommitmentPermissionsApiClient
{
    private readonly IRestHttpClient _restClient;

    public CommitmentPermissionsApiClient(IRestHttpClient restClient)
    {
        _restClient = restClient;
    }
        
    public Task<bool> CanAccessCohort(CohortAccessRequest request, CancellationToken cancellationToken = default)
    {
        return _restClient.Get<bool>("api/authorization/access-cohort", request, cancellationToken);
    }

    public Task<bool> CanAccessApprenticeship(ApprenticeshipAccessRequest request, CancellationToken cancellationToken = default)
    {
        return _restClient.Get<bool>("api/authorization/access-apprenticeship", request, cancellationToken);
    }
}