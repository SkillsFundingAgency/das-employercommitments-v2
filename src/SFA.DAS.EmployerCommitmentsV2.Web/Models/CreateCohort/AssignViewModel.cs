using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    public class AssignViewModel
    {
        public string AccountHashedId { get; set; }
        public string ReservationId { get; set; }
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
        public uint UkPrn { get; set; }

        [Required(ErrorMessage = "Select whether to add apprentices yourself or not")]
        public WhoIsAddingApprentices? WhoIsAddingApprentices { get; set; }
    }

    public enum WhoIsAddingApprentices
    {
        Employer,
        Provider
    }
}