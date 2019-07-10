using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.AddDraftApprenticeshipToNewCohort
{
    public class IndexViewModel
    {
        [FromRoute]
        public string AccountId { get; set; }
        [FromQuery]
        public string ReservationId { get; set; }
        [FromQuery]
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        [FromQuery]
        public string StartMonthYear { get; set; }
        [FromQuery]
        public string CourseCode { get; set; }
    }
}