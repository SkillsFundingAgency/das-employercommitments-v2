using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Contracts;

public interface IApprovalsApiClient
{
    Task<GetPledgeApplicationResponse> GetPledgeApplication(int pledgeApplicationId, CancellationToken cancellationToken = default);
    Task<ProviderCourseDeliveryModels> GetProviderCourseDeliveryModels(long providerId, string courseCode, long accountLegalEntityId = 0, CancellationToken cancellationToken = default);
    Task<GetEditDraftApprenticeshipResponse> GetEditDraftApprenticeship(long accountId, long cohortId, long draftApprenticeshipId, CancellationToken cancellationToken = default);
    Task<GetViewDraftApprenticeshipResponse> GetViewDraftApprenticeship(long accountId, long cohortId, long draftApprenticeshipId, CancellationToken cancellationToken = default);
    Task<GetEditApprenticeshipResponse> GetEditApprenticeship(long accountId, long apprenticeshipId, CancellationToken cancellationToken = default);
    Task<GetEditApprenticeshipDeliveryModelResponse> GetEditApprenticeshipDeliveryModel(long accountId, long apprenticeshipId, CancellationToken cancellationToken = default);
    Task<GetApprenticeshipDetailsResponse> GetApprenticeshipDetails(long providerId, long apprenticeshipId, CancellationToken cancellationToken = default);
    Task<GetEditDraftApprenticeshipSelectDeliveryModelResponse> GetEditDraftApprenticeshipSelectDeliveryModel(long providerId, long cohortId, long draftApprenticeshipId, string courseCode, CancellationToken cancellationToken = default);
    Task<GetCohortDetailsResponse> GetCohortDetails(long accountId, long cohortId, CancellationToken cancellationToken = default);
    Task<GetUserAccountsResponse> GetEmployerUserAccounts(string email, string userId);
    Task PostCohortDetails(long accountId, long cohortId, PostCohortDetailsRequest request, CancellationToken cancellationToken = default);
    Task UpdateDraftApprenticeship(long cohortId, long draftApprenticeshipId, UpdateDraftApprenticeshipApimRequest request, CancellationToken cancellationToken = default);
    Task<AddDraftApprenticeshipResponse> AddDraftApprenticeship(long cohortId, AddDraftApprenticeshipApimRequest request, CancellationToken cancellationToken = default);
    Task<CreateCohortResponse> CreateCohort(CreateCohortApimRequest request, CancellationToken cancellationToken = default);
    Task<GetManageApprenticeshipDetailsResponse> GetManageApprenticeshipDetails(long accountId, long apprenticeshipId, CancellationToken cancellationToken = default);
    Task<GetLegalEntitiesForAccountResponse> GetLegalEntitiesForAccount(string cohortId, long accountId);
    Task<GetAccountLegalEntityResponse> GetAccountLegalEntity(long accountLegalEntityId, CancellationToken cancellationToken = default);
    Task<GetSelectProviderDetailsResponse> GetSelectProviderDetails(long accountId, long accountLegalEntityId, CancellationToken cancellationToken = default);
    Task<GetSelectFundingOptionsResponse> GetSelectFundingOptions(long accountId, CancellationToken cancellationToken = default);
    Task<GetSelectDirectTransferConnectionResponse> GetSelectDirectTransferConnection(long accountId, CancellationToken cancellationToken = default);
    Task<GetFundingBandDataResponse> GetFundingBandDataByCourseCodeAndStartDate(string courseCode, DateTime? startDate, CancellationToken cancellationToken = default);
}