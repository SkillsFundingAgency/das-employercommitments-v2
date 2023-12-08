using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class ApprovedRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }
    [FromRoute]
    public string CohortReference { get; set; }
    public long CohortId { get; set; }
}