using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals
{
    public interface IApprovalsApiClient
    {
        Task<GetPledgeApplicationResponse> GetPledgeApplication(int pledgeApplicationId, CancellationToken cancellationToken = default);
        Task<ProviderCourseDeliveryModels> GetProviderCourseDeliveryModels(long providerId, string courseCode, long accountLegalEntityId = 0, CancellationToken cancellationToken = default);
        Task<GetEditDraftApprenticeshipResponse> GetEditDraftApprenticeship(long accountId, long cohortId, long draftApprenticeshipId, CancellationToken cancellationToken = default);
        Task<GetEditApprenticeshipResponse> GetEditApprenticeship(long accountId, long apprenticeshipId, CancellationToken cancellationToken = default);
        Task<GetEditApprenticeshipDeliveryModelResponse> GetEditApprenticeshipDeliveryModel(long accountId, long apprenticeshipId, CancellationToken cancellationToken = default);
        Task<GetApprenticeshipDetailsResponse> GetApprenticeshipDetails(long providerId, long apprenticeshipId, CancellationToken cancellationToken = default);
        Task<GetEditDraftApprenticeshipSelectDeliveryModelResponse> GetEditDraftApprenticeshipSelectDeliveryModel(long providerId, long cohortId, long draftApprenticeshipId, string courseCode, CancellationToken cancellationToken = default);
        Task<GetCohortDetailsResponse> GetCohortDetails(long accountId, long cohortId, CancellationToken cancellationToken = default);
        Task<GetUserAccountsResponse> GetEmployerUserAccounts(string email, string userId);
        Task PostCohortDetails(long accountId, long cohortId, PostCohortDetailsRequest request, CancellationToken cancellationToken = default);
        Task UpdateDraftApprenticeship(long cohortId, long draftApprenticeshipId, UpdateDraftApprenticeshipApimRequest request, CancellationToken cancellationToken = default);
    }
}
