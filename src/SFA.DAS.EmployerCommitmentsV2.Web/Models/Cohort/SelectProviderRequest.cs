namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class SelectProviderRequest : IndexRequest
    {
        public string TransferSenderId { get; set; }
        public Origin Origin { get; set; }
    }
}