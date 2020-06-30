using Microsoft.AspNetCore.Mvc;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Linq;

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
            else if (!cohort.IsDraft && cohort.WithParty == Party.TransferSender)
                return CohortStatus.WithTransferSender;
            else
                return CohortStatus.Unknown;
        }

        public static CohortsViewModel GetCohortCardLinkViewModel(this CohortSummary[] cohorts, IUrlHelper urlHelper, string accountHashedId, CohortStatus selectedStatus)
        {
            return new CohortsViewModel
            {
                CohortsInDraft = new CohortCardLinkViewModel(
                  cohorts.Count(x => x.GetStatus() == CohortStatus.Draft),
                  "Drafts",
                  urlHelper.Action("Draft", "Cohort", new { accountHashedId }),
                  CohortStatus.Draft.ToString(),
                  selectedStatus == CohortStatus.Draft),
                CohortsInReview = new CohortCardLinkViewModel(
                  cohorts.Count(x => x.GetStatus() == CohortStatus.Review),
                  "Ready to review",
                  urlHelper.Action("Review", "Cohort", new { accountHashedId }),
                   CohortStatus.Review.ToString(),
                   selectedStatus == CohortStatus.Review),
                CohortsWithTrainingProvider = new CohortCardLinkViewModel(
                  cohorts.Count(x => x.GetStatus() == CohortStatus.WithProvider),
                  "With training providers",
                  urlHelper.Action("WithTrainingProvider", "Cohort", new { accountHashedId }),
                  CohortStatus.WithProvider.ToString(),
                  selectedStatus == CohortStatus.WithProvider),
                CohortsWithTransferSender = new CohortCardLinkViewModel(
                  cohorts.Count(x => x.GetStatus() == CohortStatus.WithTransferSender),
                  "With transfer sending employers",
                  urlHelper.Action("WithTransferSender", "Cohort", new { accountHashedId }),
                  CohortStatus.WithTransferSender.ToString(),
                  selectedStatus == CohortStatus.WithTransferSender)
            };
        }
    }

    public enum CohortStatus
    {
        Unknown,
        Draft,
        Review,
        WithProvider,
        WithTransferSender
    }
}
