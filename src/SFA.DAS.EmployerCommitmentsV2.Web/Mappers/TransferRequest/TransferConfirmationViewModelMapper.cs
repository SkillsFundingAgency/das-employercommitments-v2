using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest
{
    public class TransferConfirmationViewModelMapper : IMapper<TransferConfirmationRequest, TransferConfirmationViewModel>
    {
        public TransferConfirmationViewModelMapper()
        {
        }

        public async Task<TransferConfirmationViewModel> Map(TransferConfirmationRequest source)
        {
            return await Task.FromResult(new TransferConfirmationViewModel
            {
                TransferApprovalStatus = source.TransferApprovalStatus == TransferApprovalStatus.Approved ? "approved" : "rejected",
                TransferReceiverName = source.TransferReceiverName
            });
        }
    }
}