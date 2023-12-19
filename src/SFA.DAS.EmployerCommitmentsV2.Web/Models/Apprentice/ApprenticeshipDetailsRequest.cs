namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ApprenticeshipDetailsRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }

    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }
}