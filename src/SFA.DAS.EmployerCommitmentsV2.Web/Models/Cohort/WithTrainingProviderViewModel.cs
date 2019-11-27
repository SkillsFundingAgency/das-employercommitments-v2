using System.Collections.Generic;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class WithTrainingProviderViewModel
    {
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public string BackLink { get; set; }
        public IEnumerable<WithTrainingProviderCohortSummaryViewModel> Cohorts { get; set; }
        
    }
}
