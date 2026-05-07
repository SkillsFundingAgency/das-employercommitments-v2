using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class FinishedRequestToFinishedViewModelMapper(IApprovalsApiClient approvalsApiClient)
    : IMapper<FinishedRequest, FinishedViewModel>
{
    public async Task<FinishedViewModel> Map(FinishedRequest request)
    {
        var response = await approvalsApiClient.GetCohortDetails(request.AccountId, request.CohortId);
        var fundingDetails = await approvalsApiClient.GetSelectFundingOptions(request.AccountId);

        return new FinishedViewModel
        {
            AccountHashedId = request.AccountHashedId,
            CohortReference = request.CohortReference,
            LegalEntityName = response.LegalEntityName,
            ProviderName = response.ProviderName,
            Message = response.LatestMessageCreatedByEmployer
        };
    }   
}