using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

public class EditDraftApprenticeshipViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel, IDraftApprenticeshipViewModel
{
    public EditDraftApprenticeshipViewModel(DateTime? dateOfBirth, DateTime? startDate, DateTime? endDate) : base(dateOfBirth, startDate, endDate)
    {
    }

    public EditDraftApprenticeshipViewModel()
    {
    }

    public Guid CacheKey { get; set; }
    public string AccountHashedId { get; set; }
    public string DraftApprenticeshipHashedId { get; set; }
    public long DraftApprenticeshipId { get; set; }
    public string LegalEntityName { get; set; }
}