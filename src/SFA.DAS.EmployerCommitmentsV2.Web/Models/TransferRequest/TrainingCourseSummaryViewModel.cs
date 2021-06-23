namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest
{
    public sealed class TrainingCourseSummaryViewModel
    {
        public string CourseTitle { get; set; }
        public int ApprenticeshipCount { get; set; }

        public string SummaryDescription
        {
            get
            {
                var label = ApprenticeshipCount == 1 ? "Apprentice" : "Apprentices";
                return $"{CourseTitle} ({ApprenticeshipCount} {label})";
            }
        }
    }
}
