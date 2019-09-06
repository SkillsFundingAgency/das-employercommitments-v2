using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class DetailsViewModel
    {
        public string AccountHashedId { get; set; }
        public Party WithParty { get; set; }
        public string CohortReference { get; set; }
    }
}
