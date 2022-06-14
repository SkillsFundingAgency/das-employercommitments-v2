﻿using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
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

        public async Task<ProviderCourseDeliveryModels> GetProviderCourseDeliveryModels(long providerId, string courseCode, CancellationToken cancellationToken = default)
        {
            return await _client.Get<ProviderCourseDeliveryModels>($"Providers/{providerId}/courses/{courseCode}", null, cancellationToken);
        }

        public async Task<GetAgencyResponse> GetAgency(int legalEntityId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetAgencyResponse>($"rofjaa/agency/{legalEntityId}", null, cancellationToken);
        }

    }
}
