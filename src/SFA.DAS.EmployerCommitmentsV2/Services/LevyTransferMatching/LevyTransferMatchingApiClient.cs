using SFA.DAS.EmployerCommitmentsV2.Services.LevyTransferMatching.Responses;
using SFA.DAS.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Services.LevyTransferMatching
{
    public class LevyTransferMatchingApiClient : ILevyTransferMatchingApiClient
    {
        private readonly IRestHttpClient _client;

        public LevyTransferMatchingApiClient(IRestHttpClient client)
        {
            _client = client;
        }

        public async Task<GetPledgeApplicationResponse> GetPledgeApplication(int pledgeApplicationId, CancellationToken cancellationToken = default)
        {
            return await _client.Get<GetPledgeApplicationResponse>($"PledgeApplications/{pledgeApplicationId}", null, cancellationToken);
        }
    }
}
