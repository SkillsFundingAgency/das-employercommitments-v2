using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class InformViewModel : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }
    public bool LevyFunded { get; set; }
}