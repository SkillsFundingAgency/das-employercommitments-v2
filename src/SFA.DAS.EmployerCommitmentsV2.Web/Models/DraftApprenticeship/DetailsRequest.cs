using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

public class DetailsRequest : IAuthorizationContextModel
{
    public long AccountId { get; set; }
    public string AccountHashedId { get; set; }
    public long CohortId { get; set; }
    public string CohortReference { get; set; }
    public long DraftApprenticeshipId { get; set; }
    public string DraftApprenticeshipHashedId { get; set; }
}

public class EditDetailsRequest : DetailsRequest
{
    public DeliveryModel? DeliveryModel { get; set; }
    public string CourseCode { get; set; }
}