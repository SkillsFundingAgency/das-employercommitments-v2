using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class ConfirmProviderRequest : BaseConfirmProviderRequest
{
    public long AccountLegalEntityId { get; set; }
}

public class BaseConfirmProviderRequest : IndexRequest
{
    public long ProviderId { get; set; }
    public string LegalEntityName { get; set; }
    public string TransferSenderId { get; set; }
    public Origin Origin { get; set; }

    [FromQuery]
    public string EncodedPledgeApplicationId { get; set; }

    public BaseConfirmProviderRequest CloneBaseValues() =>
        this.ExplicitClone();
}