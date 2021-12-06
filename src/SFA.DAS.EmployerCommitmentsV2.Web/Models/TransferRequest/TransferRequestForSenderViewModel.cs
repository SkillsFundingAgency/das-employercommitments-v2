using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest
{
    public sealed class TransferRequestForSenderViewModel : TransferRequestViewModel
    {
        public string TransferSenderHashedAccountId { get; set; }
        public string TransferReceiverPublicHashedAccountId { get; set; }
        public string TransferReceiverName { get; set; }
        public bool AutoApprovalEnabled { get; set; }
        public int PledgeApplicationId { get; set; }
        public bool AutomaticallyApproved => AutoApprovalEnabled && TransferApprovalStatus == TransferApprovalStatus.Approved;
        public bool AutomaticallyRejected => AutoApprovalEnabled && TransferApprovalStatus == TransferApprovalStatus.Rejected;
    }
}
