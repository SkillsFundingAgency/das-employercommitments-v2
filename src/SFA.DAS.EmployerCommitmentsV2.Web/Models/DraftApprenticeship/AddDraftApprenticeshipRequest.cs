using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

public class AddDraftApprenticeshipRequest : BaseAddDraftApprenticeshipRequest, IAuthorizationContextModel
{
    public long CohortId { get; set; }        
    public long AccountLegalEntityId { get; set; }
    public Guid? CacheKey { get; set; }
}

public class BaseAddDraftApprenticeshipRequest
{
    public string AccountHashedId { get; set; }
    public string CohortReference { get; set; }
    public string AccountLegalEntityHashedId { get; set; }
    public string DraftApprenticeshipHashedId { get; set; }
    public Guid ReservationId { get; set; }
    public string StartMonthYear { get; set; }
    public string CourseCode { get; set; }
    public bool AutoCreated { get; set; }
    public long ProviderId { get; set; }
    public DeliveryModel? DeliveryModel { get; set; }
    public int? Cost { get; set; }
    public int? EmploymentPrice { get; set; }
    public DateTime? EmploymentEndDate { get; set; }

    public BaseAddDraftApprenticeshipRequest CloneBaseValues() =>
        this.ExplicitClone();
}