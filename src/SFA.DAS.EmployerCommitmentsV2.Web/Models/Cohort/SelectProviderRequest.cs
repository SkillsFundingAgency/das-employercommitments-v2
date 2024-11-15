using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class SelectProviderRequest : BaseSelectProviderRequest
{
    public long AccountLegalEntityId { get; set; }
}

public class BaseSelectProviderRequest : IndexRequest
{
    public string TransferSenderId { get; set; }
    public Origin Origin { get; set; }

    [FromQuery]
    public string EncodedPledgeApplicationId { get; set; }

    public BaseSelectProviderRequest CloneBaseValues() =>
        this.ExplicitClone();
}