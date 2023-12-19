namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class StopRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }

    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }

    public long ApprenticeshipId { get; set; }

    public bool IsCoPJourney { get; set; }
}