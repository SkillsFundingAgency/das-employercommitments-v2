using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class AssignViewModel
    {
        public string AccountHashedId { get; set; }
        public Guid? ReservationId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }
        public string LegalEntityName { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
        public long ProviderId { get; set; }
        public string TransferSenderId { get; set; }
        public string EncodedPledgeApplicationId { get; set; }

        [Required(ErrorMessage = "Select whether to add apprentices yourself or not")]
        public WhoIsAddingApprentices? WhoIsAddingApprentices { get; set; }
    }

    public enum WhoIsAddingApprentices
    {
        Employer,
        Provider
    }
}