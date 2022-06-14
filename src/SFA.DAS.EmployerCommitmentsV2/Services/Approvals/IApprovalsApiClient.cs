using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals
{
    public interface IApprovalsApiClient
    {
        Task<GetPledgeApplicationResponse> GetPledgeApplication(int pledgeApplicationId, CancellationToken cancellationToken = default);
        Task<ProviderCourseDeliveryModels> GetProviderCourseDeliveryModels(long providerId, string courseCode, CancellationToken cancellationToken = default);
        Task<GetAgencyResponse> GetAgency(int legalEntityId, CancellationToken cancellationToken = default);

    }
}
