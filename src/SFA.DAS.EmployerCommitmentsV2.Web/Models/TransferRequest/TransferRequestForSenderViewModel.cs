namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest
{
    public sealed class TransferRequestForSenderViewModel : TransferRequestViewModel
    {
        public string TransferSenderHashedAccountId { get; set; }
        public string TransferReceiverPublicHashedAccountId { get; set; }
        public string TransferReceiverName { get; set; }
    }
}
