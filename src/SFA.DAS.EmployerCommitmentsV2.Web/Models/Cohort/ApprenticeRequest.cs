using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class ApprenticeRequest : BaseApprenticeRequest
{
    public long AccountLegalEntityId { get; set; }
    public long AccountId { get; set; }
}

public class BaseApprenticeRequest : IndexRequest
{
    public Guid? ApprenticeshipSessionKey { get; set; }
    public long ProviderId { get; set; }
    public string LegalEntityName { get; set; }
    public string TransferSenderId { get; set; }
    public DeliveryModel? DeliveryModel { get; set; }

    [FromQuery]
    public string EncodedPledgeApplicationId { get; set; }

    public BaseApprenticeRequest CloneBaseValues() =>
        this.ExplicitClone();
}