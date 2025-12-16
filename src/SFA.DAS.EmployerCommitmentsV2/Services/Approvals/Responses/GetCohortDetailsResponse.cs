using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetCohortDetailsResponse
{
    public long CohortId { get; set; }
    public string CohortReference { get; set; }
    public long AccountId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string LegalEntityName { get; set; }
    public string ProviderName { get; set; }
    public long? ProviderId { get; set; }
    public bool IsFundedByTransfer => TransferSenderId.HasValue;
    public long? TransferSenderId { get; set; }
    public int? PledgeApplicationId { get; set; }
    public Party WithParty { get; set; }
    public string LatestMessageCreatedByEmployer { get; set; }
    public string LatestMessageCreatedByProvider { get; set; }
    public bool IsApprovedByEmployer { get; set; }
    public bool IsApprovedByProvider { get; set; }
    public bool IsCompleteForEmployer { get; set; }
    public bool IsCompleteForProvider { get; set; }
    public ApprenticeshipEmployerType LevyStatus { get; set; }
    public long? ChangeOfPartyRequestId { get; set; }
    public bool IsLinkedToChangeOfPartyRequest => ChangeOfPartyRequestId.HasValue;
    public TransferApprovalStatus? TransferApprovalStatus { get; set; }
    public LastAction LastAction { get; set; }
    public bool ApprenticeEmailIsRequired { get; set; }
    public bool HasNoDeclaredStandards { get; set; }
    public bool HasUnavailableFlexiJobAgencyDeliveryModel { get; set; }
    public IEnumerable<string> InvalidProviderCourseCodes { get; set; }
    public IReadOnlyCollection<DraftApprenticeshipDto> DraftApprenticeships { get; set; }
    public IEnumerable<ApprenticeshipEmailOverlap> ApprenticeshipEmailOverlaps { get; set; }
    public IEnumerable<long> RplErrorDraftApprenticeshipIds { get; set; }
    public bool HasAgeRestrictedApprenticeships { get; set; }
}

public class ApprenticeshipEmailOverlap
{
    public long Id { get; set; }
    public string ErrorMessage { get; set; }
}

public class DraftApprenticeshipDto
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int? Cost { get; set; }
    public int? TrainingPrice { get; set; }
    public int? EndPointAssessmentPrice { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Uln { get; set; }
    public string CourseCode { get; set; }
    public string CourseName { get; set; }
    public DeliveryModel DeliveryModel { get; set; }
    public DateTime? OriginalStartDate { get; set; }
    public int? EmploymentPrice { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
    public bool? RecognisePriorLearning { get; set; }
    public int? DurationReducedBy { get; set; }
    public int? PriceReducedBy { get; set; }
    //TODO These need to be combined
    public bool RecognisingPriorLearningStillNeedsToBeConsidered { get; set; }
    public bool RecognisingPriorLearningExtendedStillNeedsToBeConsidered { get; set; }
    public bool? EmailAddressConfirmed { get; set; }
    public int? DurationReducedByHours { get; set; }
    public long? LearnerDataId { get; set; }
}