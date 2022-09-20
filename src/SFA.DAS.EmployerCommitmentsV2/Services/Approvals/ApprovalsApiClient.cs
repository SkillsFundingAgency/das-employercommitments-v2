using SFA.DAS.Commitments.Api.Types.Apprenticeship;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals
{
    public class ApprovalsApiClient : IApprovalsApiClient
    {
        private readonly IRestHttpClient _client;

        public ApprovalsApiClient(IRestHttpClient client)
        {
            _client = client;
        }

        public async Task<GetPledgeApplicationResponse> GetPledgeApplication(int pledgeApplicationId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetPledgeApplicationResponse>($"PledgeApplications/{pledgeApplicationId}", null, cancellationToken);
        }

        public async Task<ProviderCourseDeliveryModels> GetProviderCourseDeliveryModels(long providerId, string courseCode, long accountLegalEntityId = 0, CancellationToken cancellationToken = default)
        {
            return await _client.Get<ProviderCourseDeliveryModels>($"Providers/{providerId}/courses?trainingCode={courseCode}&accountLegalEntityId={accountLegalEntityId}", null, cancellationToken);
        }

        public async Task<GetEditDraftApprenticeshipResponse> GetEditDraftApprenticeship(long accountId, long cohortId,
            long draftApprenticeshipId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetEditDraftApprenticeshipResponse>($"employer/{accountId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit", null, cancellationToken);
        }

        public async Task<GetEditApprenticeshipResponse> GetEditApprenticeship(long accountId, long apprenticeshipId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetEditApprenticeshipResponse>($"employer/{accountId}/apprentices/{apprenticeshipId}/edit", null, cancellationToken);
        }

        public async Task<GetApprenticeshipDetailsResponse> GetApprenticeshipDetails(long providerId, long apprenticeshipId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetApprenticeshipDetailsResponse>($"provider/{providerId}/apprentices/{apprenticeshipId}/details", null, cancellationToken);
        }

        public async Task<GetEditDraftApprenticeshipSelectDeliveryModelResponse> GetEditDraftApprenticeshipSelectDeliveryModel(long providerId, long cohortId, long draftApprenticeshipId, string courseCode, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetEditDraftApprenticeshipSelectDeliveryModelResponse>($"provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit/select-delivery-model?courseCode={courseCode}", null, cancellationToken);
        }

        public async Task<GetCohortDetailsResponse> GetCohortDetails(long accountId, long cohortId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetCohortDetailsResponse>($"employer/{accountId}/unapproved/{cohortId}", null, cancellationToken);
        }
    }
}
