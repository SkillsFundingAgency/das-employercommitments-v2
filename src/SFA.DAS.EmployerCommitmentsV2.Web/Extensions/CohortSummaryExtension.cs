using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class CohortSummaryExtension
    {
        public static CohortStatus GetStatus(this CohortSummary cohort)
        {
            if (cohort.IsDraft && cohort.WithParty == Party.Employer)
                return CohortStatus.Draft;
            else if (!cohort.IsDraft && cohort.WithParty == Party.Employer)
                return CohortStatus.Review;
            else if (!cohort.IsDraft && cohort.WithParty == Party.Provider)
                return CohortStatus.WithProvider;
            else
                return CohortStatus.Unknown;
        }
    }

    public enum CohortStatus
    {
        Unknown,
        Draft,
        Review,
        WithProvider
    }
}
