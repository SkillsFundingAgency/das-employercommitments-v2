using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Encoding;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest
{
    public class TransferRequestForReceiverViewModelMapper : TransferRequestViewModelMapper<TransferRequestForReceiverViewModel>, 
        IMapper<TransferRequestRequest, TransferRequestForReceiverViewModel>
    {
        public TransferRequestForReceiverViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService)
            : base(commitmentsApiClient, encodingService)
        {
        }

        public async Task<TransferRequestForReceiverViewModel> Map(TransferRequestRequest source)
        {
            var response = await _commitmentsApiClient.GetTransferRequestForReceiver(source.AccountId, source.TransferRequestId);
            var viewModel = Map(response);
            return viewModel;
        }

        protected override TransferRequestForReceiverViewModel Map(GetTransferRequestResponse response)
        {
            var viewModel = base.Map(response);

            viewModel.TransferSenderPublicHashedAccountId = _encodingService.Encode(response.SendingEmployerAccountId, EncodingType.PublicAccountId);
            viewModel.TransferReceiverHashedAccountId = _encodingService.Encode(response.ReceivingEmployerAccountId, EncodingType.AccountId);
            viewModel.TransferSenderName = response.TransferSenderName;

            return viewModel;
        }
    }
}