using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetLegalEntitiesForAccountResponse
{
    public List<LegalEntity> LegalEntities { get; set; }
}
    
public class LegalEntity
{
    public bool HasLegalAgreement { get; set; }
    public string AccountLegalEntityPublicHashedId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string HashedAccountId { get; set; }
    public long LegalEntityId { get; set; }
    public List<Agreement> Agreements { get; set; }
}

public class Agreement
{
    public long Id { get; set; }
    public EmployerAgreementStatus Status { get; set; }
}