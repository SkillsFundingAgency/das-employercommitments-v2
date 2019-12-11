using System.Security.Policy;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class CohortCardLinkViewModel
    {
        public int Count { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        public CohortCardLinkViewModel(int count, string description, string url)
        {
            if (count > 0)
            {
                Url = url;
            }
            Count = count;
            Description = description;
        }
    }
}
