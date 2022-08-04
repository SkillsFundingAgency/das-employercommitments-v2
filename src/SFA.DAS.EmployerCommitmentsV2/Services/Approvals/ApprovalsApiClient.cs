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
    }
}
