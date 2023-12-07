using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class ConfirmDeleteViewModel : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }
    public string CohortReference { get; set; }
    public long CohortId { get; set; }
    public string ProviderName { get; set; }
    public string LegalEntityName { get; set; }
    public int DraftApprenticeshipsCount => Courses?.Sum(c => c.NumberOfDraftApprenticeships) ?? 0;
    public IReadOnlyCollection<CourseGroupingModel> Courses { get; set; }
    public bool? ConfirmDeletion { get; set; }

    public class CourseGroupingModel
    {
        public string CourseName { get; set; }
        public string DisplayCourseName => string.IsNullOrWhiteSpace(CourseName) ? "No training course" : CourseName;
        public int NumberOfDraftApprenticeships { get; set; }
    }
}