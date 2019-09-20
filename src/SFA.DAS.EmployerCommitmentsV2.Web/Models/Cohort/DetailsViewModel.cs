using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class DetailsViewModel
    {
        public string AccountHashedId { get; set; }
        public Party WithParty { get; set; }
        public string CohortReference { get; set; }
        public string LegalEntityName { get; set; }
        public string ProviderName { get; set; }
        public string Message { get; set; }
        public int DraftApprenticeshipsCount => Courses.SelectMany(c => c.DraftApprenticeships).Count();
        public IReadOnlyCollection<DetailsViewCourseGroupingModel> Courses { get; set; }
        public string PageTitle { get; set; }
    }
}
