using SFA.DAS.EmployerCommitmentsV2.Interfaces;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class OG_CacheModel : ICacheModel
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
    public string CourseCode { get; set; }
    public Guid? ReservationId { get; set; }
    public string StartMonthYear { get; set; }
    public string TransferSenderId { get; set; }
    public long ProviderId { get; set; }



}
