using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ChangeHistoryRequest : BaseChangeHistoryRequest, IAuthorizationContextModel
{
    public long ApprenticeshipId { get; set; }
    public long AccountId { get; set; }
}

public class BaseChangeHistoryRequest
{
    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }
    public string AccountHashedId { get; set; }
}