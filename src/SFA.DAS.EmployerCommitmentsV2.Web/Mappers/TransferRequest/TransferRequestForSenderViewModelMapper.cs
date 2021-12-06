using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Encoding;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest
{
    public class TransferRequestForSenderViewModelMapper : TransferRequestViewModelMapper<TransferRequestForSenderViewModel>, IMapper<TransferRequestRequest, TransferRequestForSenderViewModel>
    {
        public TransferRequestForSenderViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService)
            : base(commitmentsApiClient, encodingService)
        {
        }

        public async Task<TransferRequestForSenderViewModel> Map(TransferRequestRequest source)
        {
            var response = await _commitmentsApiClient.GetTransferRequestForSender(source.AccountId, source.TransferRequestId);
            var viewModel = Map(response);
            return viewModel;
        }

        protected override TransferRequestForSenderViewModel Map(GetTransferRequestResponse response)
        {
            var viewModel = base.Map(response);

            viewModel.TransferReceiverPublicHashedAccountId = _encodingService.Encode(response.ReceivingEmployerAccountId, EncodingType.PublicAccountId);
            viewModel.TransferSenderHashedAccountId = _encodingService.Encode(response.SendingEmployerAccountId, EncodingType.AccountId);
            viewModel.TransferReceiverName = response.LegalEntityName;
            viewModel.AutoApprovalEnabled = response.AutoApproval;
            viewModel.PledgeApplicationId = response.PledgeApplicationId;

            return viewModel;
        }
    }
}