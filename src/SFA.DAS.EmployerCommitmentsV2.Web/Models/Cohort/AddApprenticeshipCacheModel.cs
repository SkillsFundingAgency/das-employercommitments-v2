using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class AddApprenticeshipCacheModel : ICacheModel
{
    public Guid CacheKey { get; set; }
    public string AccountHashedId { get; set; }
    public long AccountId { get; set; }
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
    public DeliveryModel? DeliveryModel { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public int? BirthDay { get; set; }
    public int? BirthMonth { get; set; }
    public int? BirthYear { get; set; }

    public int? StartMonth { get; set; }
    public int? StartYear { get; set; }

    public int? EndMonth { get; set; }
    public int? EndYear { get; set; }

    public int? EmploymentEndMonth { get; set; }
    public int? EmploymentEndYear { get; set; }

    public int? Cost { get; set; }
    public int? EmploymentPrice { get; set; }
    public string Reference { get; set; }
}
