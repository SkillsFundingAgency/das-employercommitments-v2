using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class CohortsByAccountRequest : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }
    public long AccountId { get; set; }

}