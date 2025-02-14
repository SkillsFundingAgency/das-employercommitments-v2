using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SelectAcceptedLevyTransferConnectionViewModelMapper(IApprovalsApiClient approvalsApiClient, IEncodingService encodingService) : IMapper<AddApprenticeshipCacheModel, SelectAcceptedLevyTransferConnectionViewModel>
{
    public async Task<SelectAcceptedLevyTransferConnectionViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var result = await approvalsApiClient.GetSelectLevyTransferConnection(source.AccountId);

        return new SelectAcceptedLevyTransferConnectionViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipSessionKey = source.ApprenticeshipSessionKey,
            Applications = result.Applications == null ? [] :
                result.Applications.Select(x => new LevyTransferDisplayConnection()
                {
                    Id = x.Id,
                    ApplicationHashedId = GetApplicationHashedId(x),
                    SendingEmployerPublicHashedId = GetSendingEmployerPublicHashedId(x),
                    OpportunityHashedId = GetOpportunityHashedId(x),
                    DisplayName = BuildTitle(x),
                    ApplicationAndSenderHashedId = $"{GetApplicationHashedId(x)}|{GetSendingEmployerPublicHashedId(x)}"

                }).ToList()
        };
    }

    private string GetOpportunityHashedId(LevyTransferConnection x)
    {
        return encodingService.Encode(x.OpportunityId, EncodingType.PledgeId);
    }

    private string GetSendingEmployerPublicHashedId(LevyTransferConnection x)
    {
        return encodingService.Encode(x.SenderEmployerAccountId, EncodingType.PublicAccountId);
    }

    private string GetApplicationHashedId(LevyTransferConnection x)
    {
        return encodingService.Encode(x.Id, EncodingType.PledgeApplicationId);
    }

    private string BuildTitle(LevyTransferConnection levyTransferConnection)
    {
        string title = "Opportunity";
        string hashedId = GetOpportunityHashedId(levyTransferConnection);
        if (levyTransferConnection.IsNamePublic)
        {
            title = levyTransferConnection.SenderEmployerAccountName;
        }

        title += $" ({hashedId}) - £{levyTransferConnection.TotalAmount:N}";
        return title;
    }
}