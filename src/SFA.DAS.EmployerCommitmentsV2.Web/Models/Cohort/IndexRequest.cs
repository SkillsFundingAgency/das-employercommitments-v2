using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class IndexRequest
    {
        [FromRoute]
        public string AccountHashedId { get; set; }
        public string ReservationId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }

        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
    }
}