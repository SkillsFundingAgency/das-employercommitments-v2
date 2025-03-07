﻿using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetEditDraftApprenticeshipResponse
{
    public Guid? ReservationId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public string Email { get; set; }
    public string Uln { get; set; }
    public DeliveryModel DeliveryModel { get; set; }

    public string CourseCode { get; set; }
    public string StandardUId { get; set; }
    public string CourseName { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public int? Cost { get; set; }

    public int? EmploymentPrice { get; set; }

    public DateTime? EmploymentEndDate { get; set; }
    public string EmployerReference { get; set; }
    public string ProviderReference { get; set; }

    public long ProviderId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string ProviderName { get; set; }
    public string LegalEntityName { get; set; }

    public bool IsContinuation { get; set; }
    public bool HasMultipleDeliveryModelOptions { get; set; }
    public bool HasUnavailableDeliveryModel { get; set; }
    public bool? RecognisePriorLearning { get; set; }
    public int? DurationReducedBy { get; set; }
    public int? PriceReducedBy { get; set; }
    public int? TrainingTotalHours { get; set; }
    public int? DurationReducedByHours { get; set; }
    public bool? IsDurationReducedByRpl { get; set; }
    public bool? IsOnFlexiPaymentPilot { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public bool? EmailAddressConfirmed { get; set; }
    public string StandardPageUrl { get; set; }
    public int? ProposedMaxFunding { get; set; }
}