namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetProvidersListResponse
{
    public IEnumerable<GetProviderResponse> Providers { get; set; }
}

public class GetProviderResponse
{
    public string Name { get; set; }
    public int Ukprn { get; set; }
}