using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class AssignRequest : BaseAssignRequest
{
    public long AccountLegalEntityId { get; set; }
}

public class BaseAssignRequest : IndexRequest
{
    public long ProviderId { get; set; }
    public string LegalEntityName { get; set; }
    public string TransferSenderId { get; set; }
    public Origin Origin { get; set; }

    [FromQuery]
    public string EncodedPledgeApplicationId { get; set; }

    public BaseAssignRequest CloneBaseValues() =>
        this.ExplicitClone();
}