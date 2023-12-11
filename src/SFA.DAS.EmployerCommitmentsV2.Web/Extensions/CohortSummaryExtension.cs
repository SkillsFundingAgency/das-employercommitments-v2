using Microsoft.AspNetCore.Mvc;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

public enum CohortStatus
{
    Unknown,
    Draft,
    Review,
    WithProvider,
    WithTransferSender
    
}
public static class CohortSummaryExtension
{
    public static CohortStatus GetStatus(this CohortSummary cohort)
    {
        switch (cohort.IsDraft)
        {
            case true when cohort.WithParty == Party.Employer:
                return CohortStatus.Draft;
            case false when cohort.WithParty == Party.Employer:
                return CohortStatus.Review;
            case false when cohort.WithParty == Party.Provider:
                return CohortStatus.WithProvider;
            case false when cohort.WithParty == Party.TransferSender:
                return CohortStatus.WithTransferSender;
            default:
                return CohortStatus.Unknown;
        }
    }

    public static ApprenticeshipRequestsHeaderViewModel GetCohortCardLinkViewModel(this CohortSummary[] cohorts, IUrlHelper urlHelper, string accountHashedId, CohortStatus selectedStatus)
    {
        return new ApprenticeshipRequestsHeaderViewModel
        {
            AccountHashedId = accountHashedId,
            CohortsInDraft = new ApprenticeshipRequestsTabViewModel(
                cohorts.Count(x => x.GetStatus() == CohortStatus.Draft),
                cohorts.Count(x => x.GetStatus() == CohortStatus.Draft) == 1 ? "Draft" : "Drafts",
                urlHelper.Action("Draft", "Cohort", new { accountHashedId }),
                CohortStatus.Draft.ToString(),
                selectedStatus == CohortStatus.Draft),
            CohortsInReview = new ApprenticeshipRequestsTabViewModel(
                cohorts.Count(x => x.GetStatus() == CohortStatus.Review),
                "Ready to review",
                urlHelper.Action("Review", "Cohort", new { accountHashedId }),
                CohortStatus.Review.ToString(),
                selectedStatus == CohortStatus.Review),
            CohortsWithTrainingProvider = new ApprenticeshipRequestsTabViewModel(
                cohorts.Count(x => x.GetStatus() == CohortStatus.WithProvider),
                "With training providers",
                urlHelper.Action("WithTrainingProvider", "Cohort", new { accountHashedId }),
                CohortStatus.WithProvider.ToString(),
                selectedStatus == CohortStatus.WithProvider),
            CohortsWithTransferSender = new ApprenticeshipRequestsTabViewModel(
                cohorts.Count(x => x.GetStatus() == CohortStatus.WithTransferSender),
                "With transfer sending employers",
                urlHelper.Action("WithTransferSender", "Cohort", new { accountHashedId }),
                CohortStatus.WithTransferSender.ToString(),
                selectedStatus == CohortStatus.WithTransferSender)
        };
    }
}