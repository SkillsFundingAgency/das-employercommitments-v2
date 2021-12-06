using SFA.DAS.EmployerCommitmentsV2.Services.LevyTransferMatching.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Services.LevyTransferMatching
{
    public interface ILevyTransferMatchingApiClient
    {
        Task<GetPledgeApplicationResponse> GetPledgeApplication(int pledgeApplicationId, CancellationToken cancellationToken = default);
    }
}
