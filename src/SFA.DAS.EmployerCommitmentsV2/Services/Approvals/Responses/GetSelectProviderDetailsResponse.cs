namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;


public class GetSelectProviderDetailsResponse
{
    public IEnumerable<Provider> Providers { get; set; }
    public AccountLegalEntity AccountLegalEntity { get; set; }
}

public class AccountLegalEntity
{
    public string LegalEntityName { get; set; }
}

public class Provider
{
    public string Name { get; set; }
    public int Ukprn { get; set; }
}