namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    // Todo These names match the CV-193 base branch, but the names should match the ECV2 naming conventions 
    public class MessageRequest
    {
        public string AccountHashedId { get; set; }
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        public string ReservationId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
        public long UkPrn { get; set; }
    }
}