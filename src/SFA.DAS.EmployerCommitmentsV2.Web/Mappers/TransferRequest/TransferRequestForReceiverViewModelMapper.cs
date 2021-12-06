using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.LevyTransferMatching;
using SFA.DAS.EmployerCommitmentsV2.Services.LevyTransferMatching.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Encoding;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest
{
    public class TransferRequestForReceiverViewModelMapper : TransferRequestViewModelMapper<TransferRequestForReceiverViewModel>, 
        IMapper<TransferRequestRequest, TransferRequestForReceiverViewModel>
    {
        public TransferRequestForReceiverViewModelMapper(ICommitmentsApiClient commitmentsApiClient, ILevyTransferMatchingApiClient levyTransferMatchingApiClient, IEncodingService encodingService)
            : base(commitmentsApiClient, levyTransferMatchingApiClient, encodingService)
        {
        }

        public async Task<TransferRequestForReceiverViewModel> Map(TransferRequestRequest source)
        {
            var transferRequestResponse = await _commitmentsApiClient.GetTransferRequestForReceiver(source.AccountId, source.TransferRequestId);
            var pledgeApplicationResponse = await _levyTransferMatchingApiClient.GetPledgeApplication(transferRequestResponse.PledgeApplicationId);

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
}