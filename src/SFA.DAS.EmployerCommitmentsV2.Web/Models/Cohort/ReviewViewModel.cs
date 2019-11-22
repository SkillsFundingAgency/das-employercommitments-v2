using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class ReviewViewModel
    {
        public string AccountHashedId { get; set; }

        public string BackLinkUrl { get; set; }

        public IEnumerable<ReviewCohortSummaryViewModel> CohortSummary { get; set; }
    }
}
