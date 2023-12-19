using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest;

public class TransferRequestForReceiverViewModelMapper : TransferRequestViewModelMapper<TransferRequestForReceiverViewModel>, 
    IMapper<TransferRequestRequest, TransferRequestForReceiverViewModel>
{
    public TransferRequestForReceiverViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IApprovalsApiClient approvalsApiClient, IEncodingService encodingService)
        : base(commitmentsApiClient, approvalsApiClient, encodingService)
    {
    }

    public async Task<TransferRequestForReceiverViewModel> Map(TransferRequestRequest source)
    {
        var transferRequestResponse = await _commitmentsApiClient.GetTransferRequestForReceiver(source.AccountId, source.TransferRequestId);

        GetPledgeApplicationResponse pledgeApplicationResponse = null;

        if (transferRequestResponse.PledgeApplicationId.HasValue)
        {
            pledgeApplicationResponse = await _approvalsApiClient.GetPledgeApplication(transferRequestResponse.PledgeApplicationId.Value);
        }

        var viewModel = Map(transferRequestResponse, pledgeApplicationResponse);
        return viewModel;
    }

    protected override TransferRequestForReceiverViewModel Map(GetTransferRequestResponse response, GetPledgeApplicationResponse getPledgeApplicationResponse)
    {
        var viewModel = base.Map(response, getPledgeApplicationResponse);

        viewModel.TransferSenderPublicHashedAccountId = _encodingService.Encode(response.SendingEmployerAccountId, EncodingType.PublicAccountId);
        viewModel.TransferReceiverHashedAccountId = _encodingService.Encode(response.ReceivingEmployerAccountId, EncodingType.AccountId);
        viewModel.TransferSenderName = response.TransferSenderName;

        return viewModel;
    }
}