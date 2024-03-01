using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ConfirmStopRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }

    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }

    public long ApprenticeshipId { get; set; }

    public bool IsCoPJourney { get; set; }

    public int? StopMonth { get; set; }

    public int? StopYear { get; set; }

    public bool? MadeRedundant { get; set; }
}