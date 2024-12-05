using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class InformViewModel : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }
    public bool IsLevyFunded { get; set; }
    public Guid OG_CacheID { get; set; }
}