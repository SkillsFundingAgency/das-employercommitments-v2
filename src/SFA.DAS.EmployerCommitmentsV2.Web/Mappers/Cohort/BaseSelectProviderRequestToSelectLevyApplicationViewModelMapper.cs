using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class BaseSelectProviderRequestToSelectAcceptedLevyTransferConnectionViewModelMapper : IMapper<BaseSelectProviderRequest, SelectAcceptedLevyTransferConnectionViewModel>
{
    private readonly IApprovalsApiClient _approvalsApiClient;
    private readonly IEncodingService _encodingService;

    public BaseSelectProviderRequestToSelectAcceptedLevyTransferConnectionViewModelMapper(IApprovalsApiClient approvalsApiClient, IEncodingService encodingService)
    {
        _approvalsApiClient = approvalsApiClient;
        _encodingService = encodingService;
    }

    public async Task<SelectAcceptedLevyTransferConnectionViewModel> Map(BaseSelectProviderRequest source)
    {
        var result = await _approvalsApiClient.GetSelectLevyTransferConnection(source.AccountId);

        return new SelectAcceptedLevyTransferConnectionViewModel
        {
            AccountHashedId = source.AccountHashedId,
            Applications = result.Applications == null ? new List<LevyTransferDisplayConnection>() :
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
        return _encodingService.Encode(x.OpportunityId, EncodingType.PledgeId);
    }

    private string GetSendingEmployerPublicHashedId(LevyTransferConnection x)
    {
        return _encodingService.Encode(x.SenderEmployerAccountId, EncodingType.PublicAccountId);
    }

    private string GetApplicationHashedId(LevyTransferConnection x)
    {
        return _encodingService.Encode(x.Id, EncodingType.PledgeApplicationId);
    }

    private string BuildTitle(LevyTransferConnection levyTransferConnection)
    {
        string title = "Opportunity";
        string hashedId = GetOpportunityHashedId(levyTransferConnection);
        if (levyTransferConnection.IsNamePublic)
        {
            title = levyTransferConnection.SenderEmployerAccountName;
        }

        title += $" ({hashedId}) - £{levyTransferConnection.TotalAmount.ToString("N")}";
        return title;
    }
}