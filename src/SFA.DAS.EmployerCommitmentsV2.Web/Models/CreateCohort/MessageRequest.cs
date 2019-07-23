using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    public class MessageRequest : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        public string ReservationId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
        public long UkPrn { get; set; }
    }
}