namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class LegalEntitySignedAgreementViewModel
    {
        public string AccountHashedId { get; set; }
        public string TransferConnectionCode { get; set; }
        public long LegalEntityId { get; set; }
        public string CohortRef { get; set; }
        public bool HasSignedMinimumRequiredAgreementVersion { get; set; }
        public string LegalEntityName { get; set; }        
        public string AccountLegalEntityPublicHashedId { get; set; }
    }
}
