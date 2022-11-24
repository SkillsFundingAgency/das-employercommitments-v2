using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.Http;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals
{
    public class ApprovalsApiClient : IApprovalsApiClient
    {
        private readonly IOuterApiClient _client;

        public ApprovalsApiClient(IOuterApiClient client)
        {
            _client = client;
        }

        public async Task<GetPledgeApplicationResponse> GetPledgeApplication(int pledgeApplicationId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetPledgeApplicationResponse>($"PledgeApplications/{pledgeApplicationId}");
        }

        public async Task<ProviderCourseDeliveryModels> GetProviderCourseDeliveryModels(long providerId, string courseCode, long accountLegalEntityId = 0, CancellationToken cancellationToken = default)
        {
            return await _client.Get<ProviderCourseDeliveryModels>($"Providers/{providerId}/courses?trainingCode={courseCode}&accountLegalEntityId={accountLegalEntityId}");
        }

        public async Task<GetEditDraftApprenticeshipResponse> GetEditDraftApprenticeship(long accountId, long cohortId,
            long draftApprenticeshipId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetEditDraftApprenticeshipResponse>($"employer/{accountId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit");
        }

        public async Task<GetEditApprenticeshipResponse> GetEditApprenticeship(long accountId, long apprenticeshipId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetEditApprenticeshipResponse>($"employer/{accountId}/apprentices/{apprenticeshipId}/edit");
        }

        public async Task<GetEditApprenticeshipDeliveryModelResponse> GetEditApprenticeshipDeliveryModel(long accountId, long apprenticeshipId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetEditApprenticeshipDeliveryModelResponse>($"employer/{accountId}/apprentices/{apprenticeshipId}/edit/delivery-model");
        }

        public async Task<GetApprenticeshipDetailsResponse> GetApprenticeshipDetails(long providerId, long apprenticeshipId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetApprenticeshipDetailsResponse>($"provider/{providerId}/apprentices/{apprenticeshipId}/details");
        }

        public async Task<GetEditDraftApprenticeshipSelectDeliveryModelResponse> GetEditDraftApprenticeshipSelectDeliveryModel(long providerId, long cohortId, long draftApprenticeshipId, string courseCode, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetEditDraftApprenticeshipSelectDeliveryModelResponse>($"provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit/select-delivery-model?courseCode={courseCode}");
        }

        public async Task ValidateDraftApprenticeshipForOverlappingTrainingDateRequest(ValidateDraftApprenticeshipApimRequest data)
        {
            await _client.Post<object>($"OverlappingTrainingDateRequest/validate", data);
        }

        public async Task<GetCohortDetailsResponse> GetCohortDetails(long accountId, long cohortId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetCohortDetailsResponse>($"employer/{accountId}/unapproved/{cohortId}");
        }
    }
}
