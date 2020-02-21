using System.Collections.Generic;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class DraftViewModel
    {
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public IEnumerable<DraftCohortSummaryViewModel> Cohorts { get; set; }
        
    }
}
