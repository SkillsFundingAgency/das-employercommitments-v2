using System.Collections.Generic;
using Microsoft.AspNetCore.WebSockets.Internal;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class DetailsViewCourseGroupingModel
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string DisplayCourseName => string.IsNullOrWhiteSpace(CourseName) ? "No training course" : CourseName;
        public int Count => DraftApprenticeships?.Count ?? 0;
        public IReadOnlyCollection<CohortDraftApprenticeshipViewModel> DraftApprenticeships { get; set; }
    }
}