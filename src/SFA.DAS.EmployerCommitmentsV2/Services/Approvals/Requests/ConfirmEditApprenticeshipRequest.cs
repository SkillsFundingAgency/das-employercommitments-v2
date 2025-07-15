namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;

public class ConfirmEditApprenticeshipRequest
{
    public long ApprenticeshipId { get; set; }
    public long AccountId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal? Cost { get; set; }
    public string EmployerReference { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string DeliveryModel { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
    public int? EmploymentPrice { get; set; }
    public string CourseCode { get; set; }
    public string Version { get; set; }
    public string Option { get; set; }
} 