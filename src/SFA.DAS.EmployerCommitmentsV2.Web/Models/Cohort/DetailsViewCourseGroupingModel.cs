using System.Collections.Generic;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class DetailsViewCourseGroupingModel
{
    public string CourseCode { get; set; }
    public string CourseName { get; set; }
    public DeliveryModel DeliveryModel { get; set; }
    public bool IsPortableFlexiJob => DeliveryModel == DeliveryModel.PortableFlexiJob;
    public bool IsFlexiJobAgency => DeliveryModel == DeliveryModel.FlexiJobAgency;
    public string DisplayCourseName => string.IsNullOrWhiteSpace(CourseName) ? "No training course" : CourseName;
    public int Count => DraftApprenticeships?.Count ?? 0;
    public FundingBandExcessModel FundingBandExcess { get; set; }
    public EmailOverlapsModel EmailOverlaps { get; set; }
    public IReadOnlyCollection<CohortDraftApprenticeshipViewModel> DraftApprenticeships { get; set; }
    public bool ErrorIsCompletedDisplayed { get; set; }
    public bool ErrorHasOverlappingUlnDisplayed { get; set; }
    public bool ErrorEmailOverlapsDisplayed { get; set; }
    public bool ErrorFundingBandExcessDisplayed { get; set; }
}