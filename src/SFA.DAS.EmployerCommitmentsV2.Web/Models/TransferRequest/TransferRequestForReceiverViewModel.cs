namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest
{
    public sealed class TransferRequestForReceiverViewModel : TransferRequestViewModel
    {
        public string TransferReceiverHashedAccountId { get; set; }
        public string TransferSenderPublicHashedAccountId { get; set; }
        public string TransferSenderName { get; set; }
    }
}
