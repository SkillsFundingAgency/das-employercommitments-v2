namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class MessageRequest
    {
        public string AccountHashedId { get; set; }
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        public string ReservationId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
        public long ProviderId { get; set; }
    }
}