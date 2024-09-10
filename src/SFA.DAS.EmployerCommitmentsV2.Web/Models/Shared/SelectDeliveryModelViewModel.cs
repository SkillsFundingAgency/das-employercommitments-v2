using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

public class SelectDeliveryModelViewModel
{
    public string AccountHashedId { get; set; }
    public string AccountLegalEntityHashedId { get; set; }
    public long ProviderId { get; set; }
    public string CohortReference { get; set; }
    public string DraftApprenticeshipHashedId { get; set; }
    public long CohortId { get; set; }
    public Guid? ReservationId { get; set; }
    public string EmployerAccountLegalEntityPublicHashedId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string CourseCode { get; set; }
    public string StartMonthYear { get; set; }
    public DeliveryModel? DeliveryModel { get; set; }
    public DeliveryModel[] DeliveryModels { get; set; }
    public string TransferSenderId { get; set; }
    public string EncodedPledgeApplicationId { get; set; }
    public Guid CacheKey { get; set; }
}