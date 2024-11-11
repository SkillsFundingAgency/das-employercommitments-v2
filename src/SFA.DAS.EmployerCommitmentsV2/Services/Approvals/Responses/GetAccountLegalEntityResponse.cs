using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetAccountLegalEntityResponse
{
    public long AccountId { get; set; }
    public long MaLegalEntityId { get; set; }
    public string AccountName { get; set; }
    public string LegalEntityName { get; set; }
    public ApprenticeshipEmployerType LevyStatus { get; set; }
}