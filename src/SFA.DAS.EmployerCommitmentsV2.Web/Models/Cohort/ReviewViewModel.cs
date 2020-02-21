using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class ReviewViewModel
    {
        public string AccountHashedId { get; set; }
        public IEnumerable<ReviewCohortSummaryViewModel> Cohorts { get; set; }
    }
}
