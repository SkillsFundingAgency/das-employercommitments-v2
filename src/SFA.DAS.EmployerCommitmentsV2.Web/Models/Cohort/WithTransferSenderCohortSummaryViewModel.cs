namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class WithTransferSenderCohortSummaryViewModel
    {
        public long TransferSenderId { get; set; }
        public string TransferSenderName { get; set; }
        public string CohortReference { get; set; }
        public string ProviderName { get; set; }
        public int NumberOfApprentices { get; set; }
    }
}
