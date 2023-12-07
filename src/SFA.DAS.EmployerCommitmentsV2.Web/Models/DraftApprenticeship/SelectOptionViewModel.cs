using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

public class SelectOptionViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel
{
    public SelectOptionViewModel(DateTime? dateOfBirth, DateTime? startDate, DateTime? endDate) : base(dateOfBirth, startDate, endDate)
    {
    }

    public SelectOptionViewModel()
    {
    }

    public string AccountHashedId { get; set; }
    public long DraftApprenticeshipId { get; set; }
    public string ApprenticeshipHashedId { get; set; }

    public string StandardTitle { get; set; }
    public string Version { get; set; }
    public bool CourseVersionConfirmed { get; set; }
    public string StandardUrl { get; set; }
    public IEnumerable<string> Options { get; set; }
}