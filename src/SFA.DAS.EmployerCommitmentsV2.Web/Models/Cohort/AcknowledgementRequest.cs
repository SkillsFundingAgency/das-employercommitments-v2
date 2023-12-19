namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class AcknowledgementRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }
    [FromRoute]
    public string CohortReference { get; set; }
    public long CohortId { get; set; }
}