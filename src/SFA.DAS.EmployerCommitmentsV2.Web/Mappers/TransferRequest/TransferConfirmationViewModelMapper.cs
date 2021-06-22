using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest
{
    public class TransferConfirmationViewModelMapper : IMapper<TransferConfirmationRequest, TransferConfirmationViewModel>
    {
        protected readonly ICommitmentsApiClient _commitmentsApiClient;

        public TransferConfirmationViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<TransferConfirmationViewModel> Map(TransferConfirmationRequest source)
        {
            var response = await _commitmentsApiClient.GetTransferRequestForSender(source.AccountId, source.TransferRequestId);
            
            return new TransferConfirmationViewModel
            {
                TransferApprovalStatus = response.Status == TransferApprovalStatus.Approved ? "approved" : "rejected",
                TransferReceiverName = response.LegalEntityName
            };
        }
    }
}