﻿using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class DetailsViewCourseGroupingModel
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string DisplayCourseName => string.IsNullOrWhiteSpace(CourseName) ? "No training course" : CourseName;
        public int Count => DraftApprenticeships?.Count ?? 0;
        public FundingBandExcessModel FundingBandExcess { get; set; }
        public IReadOnlyCollection<CohortDraftApprenticeshipViewModel> DraftApprenticeships { get; set; }
    }
}