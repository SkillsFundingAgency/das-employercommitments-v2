using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class SelectLegalEntityRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }

    public string transferConnectionCode { get; set; }

    public string cohortRef { get; set; }

    [FromQuery]
    public string EncodedPledgeApplicationId { get; set; }
}