namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class ValidateEditApprenticeshipResponse
{
    public long ApprenticeshipId { get; set; }
    public bool HasOptions { get; set; }
    public bool CourseOrStartDateChange { get; set; }
    public string Version { get; set; }
} 