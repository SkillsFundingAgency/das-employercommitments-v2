using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;

namespace SFA.DAS.Commitments.Shared.Extensions
{
    public interface IProviderCommitmentsService
    {
        Task<CohortDetails> GetCohortDetail(long cohortId);
        Task AddDraftApprenticeshipToCohort(long cohortId, AddDraftApprenticeshipRequest request);
        Task<EditDraftApprenticeshipDetails> GetDraftApprenticeshipForCohort(long cohortId, long draftApprenticeshipId);
        Task UpdateDraftApprenticeship(long cohortId, long draftApprenticeshipId, UpdateDraftApprenticeshipRequest updateRequest);
    }
}