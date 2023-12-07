using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest;

public class TransferRequestForSenderViewModelMapper : TransferRequestViewModelMapper<TransferRequestForSenderViewModel>, IMapper<TransferRequestRequest, TransferRequestForSenderViewModel>
{
    public TransferRequestForSenderViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IApprovalsApiClient approvalsApiClient, IEncodingService encodingService)
        : base(commitmentsApiClient, approvalsApiClient, encodingService)
    {
    }

    public async Task<TransferRequestForSenderViewModel> Map(TransferRequestRequest source)
    {
        var transferRequestResponse = await _commitmentsApiClient.GetTransferRequestForSender(source.AccountId, source.TransferRequestId);

        GetPledgeApplicationResponse pledgeApplicationResponse = null;

        if (transferRequestResponse.PledgeApplicationId.HasValue)
        {
            pledgeApplicationResponse = await _approvalsApiClient.GetPledgeApplication(transferRequestResponse.PledgeApplicationId.Value);
        }

        var viewModel = Map(transferRequestResponse, pledgeApplicationResponse);
        return viewModel;
    }

    protected override TransferRequestForSenderViewModel Map(GetTransferRequestResponse transferRequestResponse, GetPledgeApplicationResponse getPledgeApplicationResponse)
    {
        var viewModel = base.Map(transferRequestResponse, getPledgeApplicationResponse);

        viewModel.TransferReceiverPublicHashedAccountId = _encodingService.Encode(transferRequestResponse.ReceivingEmployerAccountId, EncodingType.PublicAccountId);
        viewModel.TransferSenderHashedAccountId = _encodingService.Encode(transferRequestResponse.SendingEmployerAccountId, EncodingType.AccountId);
        viewModel.TransferReceiverName = transferRequestResponse.LegalEntityName;

        return viewModel;
    }
}