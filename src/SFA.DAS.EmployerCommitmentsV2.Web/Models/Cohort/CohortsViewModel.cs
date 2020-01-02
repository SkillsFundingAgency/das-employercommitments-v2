namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class CohortsViewModel
    {
        public string AccountHashedId { get; set; }
        public CohortCardLinkViewModel CohortsInDraft { get; set; }
        public CohortCardLinkViewModel CohortsInReview { get; set; }
        public CohortCardLinkViewModel CohortsWithTrainingProvider { get; set; }
        public CohortCardLinkViewModel CohortsWithTransferSender { get; set; }
    }
}
