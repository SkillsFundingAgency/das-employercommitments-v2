using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

public class SelectOptionRequest : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }
    public long CohortId { get; set; }
    public string CohortReference { get; set; }
    public long DraftApprenticeshipId { get; set; }
}