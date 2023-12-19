using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class ViewEmployerAgreementModel :  IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }

    public long CohortId { get; set; }
}