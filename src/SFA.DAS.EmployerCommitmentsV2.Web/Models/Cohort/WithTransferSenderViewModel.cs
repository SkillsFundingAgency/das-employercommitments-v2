using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class WithTransferSenderViewModel
    {
        public string Title { get; set; }
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public IEnumerable<WithTransferSenderCohortSummaryViewModel> Cohorts { get; set; }
    }
}
