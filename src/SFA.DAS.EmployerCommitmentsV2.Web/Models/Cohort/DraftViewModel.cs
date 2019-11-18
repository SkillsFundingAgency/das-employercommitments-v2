using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class DraftViewModel
    {
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public IEnumerable<CohortSummary> Cohorts { get; set; }
        
    }
}
