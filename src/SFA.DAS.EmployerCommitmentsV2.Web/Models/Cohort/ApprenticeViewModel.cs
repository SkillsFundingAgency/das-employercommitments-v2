using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class ApprenticeViewModel : DraftApprenticeshipViewModel
{
    public long AccountId { get; set; }
    public string AccountHashedId { get; set; }
    public string LegalEntityName { get; set; }
    public string TransferSenderId { get; set; }
    public long? DecodedTransferSenderId { get; set; }
    public string EncodedPledgeApplicationId { get; set; }
    public long? PledgeApplicationId { get; set; }
    public Guid? AddApprenticeshipCacheKey { get; set; }
}