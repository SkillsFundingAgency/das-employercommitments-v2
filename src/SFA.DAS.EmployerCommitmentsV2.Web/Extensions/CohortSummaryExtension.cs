using SFA.DAS.CommitmentsV2.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class CohortSummaryExtension
    {
        public static IEnumerable<CohortSummary> Filter(this IEnumerable<CohortSummary> cohorts,  CohortStatus status)
        {
            switch (status)
            {
                case CohortStatus.Draft:
                    return cohorts.Where(x => x.IsDraft && x.WithParty == Party.Employer);
                case CohortStatus.Review:
                    return cohorts.Where(x => !x.IsDraft && x.WithParty == Party.Employer);
            }

            return null;
        }

        public static bool IsStatus(CohortSummary cohort, CohortStatus status)
        {
            switch (status)
            {
                case CohortStatus.Draft:
                    return cohort.IsDraft && cohort.WithParty == Party.Employer;
                case CohortStatus.Review:
                    return cohort.IsDraft && cohort.WithParty == Party.Employer;
            }

            return false;
        }
    }

    public enum CohortStatus
    {
        Draft,
        Review
    }
}
