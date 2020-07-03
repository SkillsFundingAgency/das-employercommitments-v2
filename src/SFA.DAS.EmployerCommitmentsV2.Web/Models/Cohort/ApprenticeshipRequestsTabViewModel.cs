namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class ApprenticeshipRequestsTabViewModel
    {
        public int Count { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string LinkId { get; set; }
        public bool IsSelected { get; set; }

        public ApprenticeshipRequestsTabViewModel(int count, string description, string url, string linkId, bool selected = false)
        {
            Url = url;
            Count = count;
            Description = description;
            LinkId = linkId;
            IsSelected = selected;
        }
    }
}
