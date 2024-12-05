namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class OG_CacheModel
{
    public Guid CacheKey { get; set; }
    public string AccountHashedId { get; set; }
    public long AccountId { get; set; }
    public string TransferConnectionCode { get; set; }
    public string CohortRef { get; set; }
    public string EncodedPledgeApplicationId { get; set; } = null;
    public string AccountLegalEntityHashedId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string LegalEntityName { get; set; }


}
