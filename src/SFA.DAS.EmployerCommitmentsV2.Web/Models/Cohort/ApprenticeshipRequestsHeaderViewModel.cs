namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class ApprenticeshipRequestsHeaderViewModel
    {
        public string AccountHashedId { get; set; }
        public ApprenticeshipRequestsTabViewModel CohortsInDraft { get; set; }
        public ApprenticeshipRequestsTabViewModel CohortsInReview { get; set; }
        public ApprenticeshipRequestsTabViewModel CohortsWithTrainingProvider { get; set; }
        public ApprenticeshipRequestsTabViewModel CohortsWithTransferSender { get; set; }
    }
}
