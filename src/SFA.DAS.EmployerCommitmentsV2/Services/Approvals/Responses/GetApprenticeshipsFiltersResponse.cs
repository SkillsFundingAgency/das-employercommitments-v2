namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetApprenticeshipsFiltersResponse
{
    public IEnumerable<string> ProviderNames { get; set; }
    public IEnumerable<string> CourseNames { get; set; }
    public IEnumerable<DateTime> EndDates { get; set; }
}