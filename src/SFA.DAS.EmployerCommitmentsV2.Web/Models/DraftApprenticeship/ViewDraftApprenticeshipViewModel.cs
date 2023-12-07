using System;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

public class ViewDraftApprenticeshipViewModel : IDraftApprenticeshipViewModel
{
    public string AccountHashedId { get; set; }
    public string CohortReference { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Uln { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DeliveryModel? DeliveryModel { get; set; }
    public string TrainingCourse { get; set; }
    public string Version { get; set; }
    public string CourseOption { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
    public int? Cost { get; set; }
    public int? EmploymentPrice { get; set; }
    public string Reference { get; set; }
    public string LegalEntityName { get; set; }
    public bool HasStandardOptions { get; set; }
    public bool? RecognisePriorLearning { get; set; }
    public int? DurationReducedBy { get; set; }
    public int? PriceReducedBy { get; set; }
    public bool? IsOnFlexiPaymentPilot { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public int? DurationReducedByHours { get; set; }
    public int? TrainingTotalHours { get; set; }
}