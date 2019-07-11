namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    public class SelectProviderViewModel
    {
        public string AccountId { get; set; }
       
        public string ReservationId { get; set; }
       
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
       
       public string StartMonthYear { get; set; }

        public string CourseCode { get; set; }

        public long ProviderId { get; set; }
    }
}
