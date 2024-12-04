using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class BaseSelectProviderRequestToSelectTransferConnectionViewModelMapper : IMapper<BaseSelectProviderRequest, SelectTransferConnectionViewModel>
{
    private readonly IApprovalsApiClient _approvalsApiClient;
    private readonly IEncodingService _encodingService;

    public BaseSelectProviderRequestToSelectTransferConnectionViewModelMapper(IApprovalsApiClient approvalsApiClient, IEncodingService encodingService)
    {
        _approvalsApiClient = approvalsApiClient;
        _encodingService = encodingService;
    }

    public async Task<SelectTransferConnectionViewModel> Map(BaseSelectProviderRequest source)
    {
        var result = await _approvalsApiClient.GetSelectDirectTransferConnection(source.AccountId);

        return new SelectTransferConnectionViewModel
        {
            AccountHashedId = source.AccountHashedId,
            IsLevyAccount = result.IsLevyAccount,
            TransferConnections = result.TransferConnections == null ? new List<TransferConnection>() : 
                result.TransferConnections.Select(x=> new TransferConnection
                {
                    FundingEmployerAccountId = x.FundingEmployerAccountId,
                    FundingEmployerPublicHashedAccountId = _encodingService.Encode(x.FundingEmployerAccountId, EncodingType.PublicAccountId),
                    FundingEmployerHashedAccountId = _encodingService.Encode(x.FundingEmployerAccountId, EncodingType.AccountId),
                    FundingEmployerAccountName = x.FundingEmployerAccountName,
                }).ToList()
        };
    }
}