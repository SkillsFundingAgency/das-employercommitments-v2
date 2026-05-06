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
            Message = response.LatestMessageCreatedByEmployer,
            FundingSource = GetFundingType(fundingDetails)
        };
    }

    public string GetFundingType(GetSelectFundingOptionsResponse response)
    {
        if (response.IsLevyAccount)
        {
            return "Current levy funds";
        }
        if (!response.IsLevyAccount && response.HasUnallocatedReservationsAvailable)
        {
            return "Reserved funds";
        }
        if (response.HasLtmTransfersAvailable)
        {
            return "Transfer funds";
        }
        if (response.HasDirectTransfersAvailable)
        {
            return "Transfer funds from a connection";
        }
        if (!response.IsLevyAccount && response.HasAdditionalReservationFundsAvailable)
        {
            return "Reserve new funds";
        }

        return "Reserved funds";
    }
}