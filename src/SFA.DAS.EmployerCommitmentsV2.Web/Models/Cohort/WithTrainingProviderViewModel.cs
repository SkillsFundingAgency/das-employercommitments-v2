using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class WithTrainingProviderViewModel
    {
        public string Title { get; set; }
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public IEnumerable<WithTrainingProviderCohortSummaryViewModel> Cohorts { get; set; }
    }
}
