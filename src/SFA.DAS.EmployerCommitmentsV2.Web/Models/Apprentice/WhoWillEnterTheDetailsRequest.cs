namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class WhoWillEnterTheDetailsRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }
    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }

    public long ProviderId { get; set; }
}