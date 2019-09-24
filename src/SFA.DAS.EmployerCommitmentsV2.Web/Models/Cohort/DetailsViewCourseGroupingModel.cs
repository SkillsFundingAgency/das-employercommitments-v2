using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebSockets.Internal;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class DetailsViewCourseGroupingModel
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string DisplayCourseName => string.IsNullOrWhiteSpace(CourseName) ? "No training course" : CourseName;
        public int Count => DraftApprenticeships?.Count ?? 0;
        public FundingBandExcessModel FundingBandExcess { get; set; }
        public List<CohortDraftApprenticeshipViewModel> DraftApprenticeships { get; set; }
    }
}