using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    public class IndexRequest
    {
        [FromRoute]
        [Required]
        public string AccountHashedId { get; set; }
        
        [Required]
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        
        public string ReservationId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
    }
}